import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import api from "../services/api";
import StatusBar from "../components/StatusBar";
import CombatLog from "../components/CombatLog";
import styles from "./MissionControlPage.module.css";


const simularTurnoCombate = (acaoId, patoAtual) => {
   const log = [];
   let novoPato = { ...patoAtual };
   let statusJogo = "em_andamento";
   let danoCausado = 0;
   const acaoNome =
      ACOES_DISPONIVEIS.find((a) => a.id === acaoId)?.nome || acaoId;

   switch (acaoId) {
      case "ataque_padrao":
         danoCausado = Math.floor(Math.random() * 15) + 10;
         log.push(`Drone usou ${acaoNome} causando ${danoCausado} de dano.`);
         break;
      case "ataque_pedra":
         danoCausado =
            (novoPato.integridade_max || 150) > 100
               ? Math.floor(Math.random() * 30) + 20
               : Math.floor(Math.random() * 10) + 5;
         log.push(`Drone usou ${acaoNome} causando ${danoCausado} de dano!`);
         break;
      case "scan_fraqueza":
         const fraquezas = [
            "Chocolate",
            "Ruído metálico",
            "Ondas sonoras agudas",
            "Efeito espelho",
            "Presença de crianças em festas",
         ];
         log.push(
            `Scan: Fraqueza detectada - ${
               fraquezas[Math.floor(Math.random() * fraquezas.length)]
            }.`
         );
         break;
      default:
         log.push(`Ação '${acaoNome}' executada.`);
   }

   if (danoCausado > 0) {
      novoPato.integridade = Math.max(0, novoPato.integridade - danoCausado);
   }
   if (
      Math.random() < 0.2 &&
      acaoId !== "scan_fraqueza" &&
      acaoId !== "mover"
   ) {
      const defesas = [
         "Gerou escudo!",
         "Ativou iscas!",
         "Teleportou brigadeiros!",
         "Fumaça!",
         "Sinal robótico!",
      ];
      log.push(
         `🌀 Defesa aleatória ativada: ${
            defesas[Math.floor(Math.random() * defesas.length)]
         }`
      );
   }
   if (novoPato.integridade <= 0) {
      statusJogo = "vitoria";
      log.push(">> ALVO NEUTRALIZADO! <<");
   }

   return {
      novoPatoStats: novoPato,
      logEntries: log,
      status: statusJogo, 
   };
};

const ACOES_DISPONIVEIS = [
   { id: "ataque_padrao", nome: "Ataque Padrão" },
   { id: "ataque_pedra", nome: "Ataque Aéreo" },
   { id: "scan_fraqueza", nome: "Escanear Fraqueza" },
   { id: "mover", nome: "Mover Drone" },
];


const MissionControlPage = () => {
   const { patoId } = useParams();
   const navigate = useNavigate();

   const [droneStats, setDroneStats] = useState(null); 
   const [patoStats, setPatoStats] = useState(null); 
   const [acoes, setAcoes] = useState(ACOES_DISPONIVEIS);
   const [combatLog, setCombatLog] = useState(["Iniciando conexão..."]);
   const [gameStatus, setGameStatus] = useState("carregando");
   const [isProcessing, setIsProcessing] = useState(false);
   const [error, setError] = useState(null);
   const [isLoading, setIsLoading] = useState(true);

   const numeroSerieDrone = "DRONE-001"; 

   
   const fetchDroneStatus = async (showLog = false) => {
      if (showLog)
         setCombatLog((prev) => [
            ...prev.slice(-100),
            "Buscando status atualizado do drone...",
         ]);
      try {
         const response = await api.get(
            `/DronesOperacionais/${numeroSerieDrone}/status`
         );
         const droneData = {
            numeroSerie: response.data.numeroSerie,
            integridade: response.data.integridadePorcentagem,
            integridade_max: 100,
            bateria: response.data.bateriaPorcentagem,
            bateria_max: 100,
            combustivel: response.data.combustivelPorcentagem,
            combustivel_max: 100,
            latitude: response.data.latitude,
            longitude: response.data.longitude,
            podeOperar: response.data.podeOperar,
         };
         setDroneStats(droneData); 
         if (showLog)
            setCombatLog((prev) => [
               ...prev.slice(-100),
               "Status do drone recebido.",
            ]);

         if (!droneData.podeOperar || droneData.integridade <= 0) {
            if (gameStatus !== "derrota" && gameStatus !== "vitoria") {
               
               setGameStatus("derrota");
               setCombatLog((prev) => [
                  ...prev.slice(-100),
                  ">> NÍVEIS CRÍTICOS OU DRONE DESTRUÍDO. Missão falhou. <<",
               ]);
            }
         }
         return droneData; 
      } catch (err) {
         console.error("Falha ao buscar status do drone", err);
         setError("Erro ao comunicar com o drone.");
         setCombatLog((prev) => [
            ...prev.slice(-100),
            ">> ERRO ao buscar status do drone. <<",
         ]);
         setGameStatus("erro");
         return null;
      }
   };

   useEffect(() => {
      const iniciarMissao = async () => {
         setIsLoading(true);
         setError(null);
         setCombatLog(["Iniciando conexão..."]);
         setGameStatus("carregando");
         let initialDroneData = null; 

         try {
            initialDroneData = await fetchDroneStatus(true);
            if (!initialDroneData)
               throw new Error(
                  "Não foi possível obter status inicial do drone."
               );
            if (!initialDroneData.podeOperar) {
               throw new Error("Drone inicializou inoperante.");
            }

            const patoResponse = await api.get(`/patos/${patoId}`);
            const patoDataApi = patoResponse.data;
            const patoDataInicial = {
               ...patoDataApi,
               nome:
                  patoDataApi.pontoReferencia ||
                  `Pato ${patoId.substring(0, 8)}`,
               integridade: 150, 
               integridade_max: 150,
               super_poder: patoDataApi.poderNome,
               status: patoDataApi.status, 
            };
            setPatoStats(patoDataInicial);
            setCombatLog((prev) => [
               ...prev.slice(-100),
               `Alvo ${patoDataInicial.nome} localizado.`,
            ]);

            setGameStatus("em_andamento");
         } catch (err) {
            console.error("Falha ao iniciar missão", err);
            setCombatLog((prev) => [
               ...prev.slice(-100),
               `ERRO CRÍTICO: ${err.message}`,
            ]);
            setError(err.message || "Falha ao buscar dados iniciais.");
            setGameStatus("erro");
            if (initialDroneData) setDroneStats(initialDroneData);
         } finally {
            setIsLoading(false);
         }
      };

      iniciarMissao();
   }, [patoId]);

   const handleAction = async (acaoId) => {
      if (isProcessing || gameStatus !== "em_andamento") return;
      setIsProcessing(true);
      setError(null);

      const acao = ACOES_DISPONIVEIS.find((a) => a.id === acaoId);
      const acaoNome = acao?.nome || acaoId;
      setCombatLog((prev) => [
         ...prev.slice(-100),
         `Executando: ${acaoNome}...`,
      ]);

      let logAcaoEspecifica = []; 

      try {
         if (acaoId === "mover") {
            const moveData = {
               latitudeDestino: (droneStats?.latitude || 0) + 0.001,
               longitudeDestino: (droneStats?.longitude || 0) + 0.001,
               velocidadeAlvo: 50,
               altitudeAlvo: droneStats?.altitudeAtual || 50,
            };
            await api.post(
               `/DronesOperacionais/${numeroSerieDrone}/mover`,
               moveData
            );
            logAcaoEspecifica.push("Comando de movimento enviado com sucesso.");
         } else if (acaoId === "scan_fraqueza") {
            try {
               const analysisResponse = await api.post(
                  `/DronesOperacionais/${numeroSerieDrone}/analisar-pato/${patoId}`
               );
               logAcaoEspecifica.push("--- Relatório de Análise ---");
               if (
                  analysisResponse.data &&
                  Array.isArray(analysisResponse.data) &&
                  analysisResponse.data.length > 0
               ) {
                  analysisResponse.data.forEach((item) => {
                     logAcaoEspecifica.push(
                        `Tipo: ${item.tipo} | Efet.: ${
                           item.efetividade
                        }% | Tática: ${
                           item.taticasRecomendadas?.join(", ") || "N/A"
                        }`
                     );
                  });
               } else {
                  logAcaoEspecifica.push("Análise inconclusiva.");
               }
               logAcaoEspecifica.push("--------------------------");
            } catch (scanErr) {
               console.warn(
                  "API /analisar-pato não encontrada ou falhou, simulando scan:",
                  scanErr
               );
               const fraquezas = [
                  "Chocolate",
                  "Ruído",
                  "Ondas agudas",
                  "Espelho",
                  "Crianças",
               ];
               logAcaoEspecifica.push(
                  `Scan simulado: Fraqueza - ${
                     fraquezas[Math.floor(Math.random() * fraquezas.length)]
                  }.`
               );
            }
         } else {
            const resultadoTurnoPato = simularTurnoCombate(acaoId, patoStats);
            logAcaoEspecifica.push(...resultadoTurnoPato.logEntries);
            setPatoStats(resultadoTurnoPato.novoPatoStats); 
            const logContemAlerta = resultadoTurnoPato.logEntries.some((log) =>
               log.startsWith("ALERTA!")
            );
            if (logContemAlerta && patoStats.status === "Desperto") {
               const danoSimulado = Math.floor(Math.random() * 15) + 5; 
               const percentualDano = (danoSimulado / 100) * 100; 
               try {
                  await api.post(
                     `/DronesOperacionais/${numeroSerieDrone}/registrar-dano`,
                     { percentualDano: Math.max(1, percentualDano) }
                  );
                  logAcaoEspecifica.push(
                     `Dano recebido (${percentualDano.toFixed(
                        0
                     )}%) registrado na API.`
                  );
               } catch (danoErr) {}
            }

            if (resultadoTurnoPato.status !== gameStatus) {
               setGameStatus(resultadoTurnoPato.status);
            }
         }

         setCombatLog((prev) => [...prev.slice(-100), ...logAcaoEspecifica]); 
         await fetchDroneStatus(false); 
      } catch (err) {
         console.error(`Erro ao executar ação ${acaoId}:`, err);
         const errorMsg = `>> Falha ao executar ${acaoNome}. <<`;
         setCombatLog((prev) => [...prev.slice(-100), errorMsg]);
         setError(`Falha ao executar ${acaoNome}.`);
      } finally {
         await new Promise((res) => setTimeout(res, 300));
         setIsProcessing(false);
      }
   };

   if (isLoading) {
      /* ... JSX Loading ... */
   }
   if (error && gameStatus === "erro") {
      /* ... JSX Erro ... */
   }
   if (!droneStats || !patoStats) {
      return (
         <div className={styles.loadingContainer}>
            <h1>Inicializando...</h1>
            <p>Aguardando dados.</p>
         </div>
      );
   }

   // --- JSX Principal ---
   return (
      <div className={styles.missionContainer}>
         {/* Overlays Vitória/Derrota */}
         {gameStatus === "vitoria" && (
            <div className={styles.endGameOverlay}> {/* ... */} </div>
         )}
         {gameStatus === "derrota" && (
            <div className={styles.endGameOverlay}>
               <h1 className={styles.derrota}>MISSÃO FALHOU</h1>
               <p>
                  {!droneStats?.podeOperar
                     ? "Níveis críticos do drone atingidos."
                     : "O drone foi destruído."}
               </p>
               <div className={styles.endGameButtons}>
                  <button onClick={() => navigate("/patopedia")}>Voltar</button>
                  <button
                     className={styles.retryButtonEnd}
                     onClick={() => window.location.reload()}
                  >
                     Tentar Novamente
                  </button>
               </div>
            </div>
         )}

         {/* Painéis de Status */}
         <div className={styles.statusPanels}>
            <div className={styles.panel}>
               <h2>DRONE: {numeroSerieDrone}</h2>
               <StatusBar
                  label="Integridade"
                  current={droneStats.integridade}
                  max={100}
                  color={
                     droneStats.integridade < 30
                        ? "var(--color-danger)"
                        : "var(--color-accent)"
                  }
               />
               <StatusBar
                  label="Bateria"
                  current={droneStats.bateria}
                  max={100}
                  color={
                     droneStats.bateria < 20 ? "var(--color-danger)" : "#3b82f6"
                  }
               />
               <StatusBar
                  label="Combustível"
                  current={droneStats.combustivel}
                  max={100}
                  color={
                     droneStats.combustivel < 15
                        ? "var(--color-danger)"
                        : "#f59e0b"
                  }
               />
               <p className={styles.droneCoords}>
                  Pos: ({droneStats.latitude?.toFixed(4)},{" "}
                  {droneStats.longitude?.toFixed(4)})
               </p>
            </div>
            <div className={styles.panel}>
               <h2>ALVO: {patoStats.nome}</h2>
               <StatusBar
                  label="Integridade Alvo"
                  current={patoStats.integridade}
                  max={patoStats.integridade_max}
                  color="var(--color-danger)"
               />
               {patoStats.super_poder && (
                  <p className={styles.patoInfo}>
                     Poder: <span>{patoStats.super_poder}</span>
                  </p>
               )}
               <p className={styles.patoInfo}>
                  Status: <span>{patoStats.status}</span>
               </p>
            </div>
         </div>

         {/* Log */}
         <div className={styles.logPanel}>
            <CombatLog logs={combatLog} />
         </div>

         {/* Ações */}
         <div className={styles.actionBar}>
            <div className={styles.mainActions}>
               {acoes.map((acao) => (
                  <button
                     key={acao.id}
                     className={styles.actionButton}
                     onClick={() => handleAction(acao.id)}
                     disabled={
                        isProcessing ||
                        gameStatus !== "em_andamento" ||
                        !droneStats?.podeOperar
                     }
                  >
                     {acao.nome}
                  </button>
               ))}
            </div>
            {/* Botão Tentar Novamente */}
            <button
               className={styles.retryButton}
               onClick={() => window.location.reload()}
               style={{
                  display:
                     gameStatus !== "carregando" &&
                     gameStatus !== "em_andamento"
                        ? "block"
                        : "none",
               }}
            >
               Tentar novamente
            </button>
         </div>
      </div>
   );
};

export default MissionControlPage;
