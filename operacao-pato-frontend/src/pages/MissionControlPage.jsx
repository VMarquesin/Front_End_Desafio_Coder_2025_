// MissionControlPage.jsx (Versão Completa e Corrigida)

import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import api from "../services/api";
import StatusBar from "../components/StatusBar";
import CombatLog from "../components/CombatLog";
import styles from "./MissionControlPage.module.css";

// 1. FUNÇÃO AUXILIAR (NECESSÁRIA)
const formatarDroneData = (apiData) => {
   if (!apiData) return null;
   return {
      numeroSerie: apiData.numeroSerie,
      integridade: apiData.integridadePorcentagem,
      integridade_max: 100,
      bateria: apiData.bateriaPorcentagem,
      bateria_max: 100,
      combustivel: apiData.combustivelPorcentagem,
      combustivel_max: 100,
      latitude: apiData.latitude,
      longitude: apiData.longitude,
      podeOperar: apiData.podeOperar,
   };
};

// 2. CONSTANTES DO JOGO
const ACOES_DISPONIVEIS = [
   { id: "ataque_padrao", nome: "Ataque Padrão" },
   { id: "ataque_pedra", nome: "Ataque Aéreo" },
   { id: "scan_fraqueza", nome: "Escanear Fraqueza" },
   { id: "mover", nome: "Mover Drone" },
];

// 3. SIMULAÇÃO DE COMBATE (COM CONTRA-ATAQUE)
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
      default:
         log.push(`Ação '${acaoNome}' executada (sem dano).`);
   }

   if (danoCausado > 0) {
      novoPato.integridade = Math.max(0, novoPato.integridade - danoCausado);
   }

   // --- LÓGICA DE CONTRA-ATAQUE DO PATO ---
   if (
      Math.random() < 0.4 && // 40% de chance de revidar
      acaoId !== "scan_fraqueza" &&
      acaoId !== "mover" &&
      novoPato.integridade > 0 &&
      novoPato.status === "Desperto"
   ) {
      const ataquesPato = [
         "Investida Quack!",
         "Onda Sônica!",
         "Projétil de Pão!",
      ];
      const ataque =
         ataquesPato[Math.floor(Math.random() * ataquesPato.length)];
      log.push(`ALERTA! O Pato revidou com ${ataque}!`);
   }
   // --- FIM DA LÓGICA ---

   if (novoPato.integridade <= 0 && statusJogo !== "vitoria") {
      statusJogo = "vitoria";
      log.push(">> ALVO NEUTRALIZADO! <<");
   }

   return {
      novoPatoStats: novoPato,
      logEntries: log,
      status: statusJogo,
   };
};

// 4. COMPONENTE PRINCIPAL
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

   // 5. FUNÇÃO DE STATUS (SIMULADA)
   const fetchDroneStatus = async (showLog = false) => {
      if (showLog)
         setCombatLog((prev) => [
            ...prev.slice(-100),
            "Simulando verificação de status do drone...",
         ]);

      setDroneStats((prev) => {
         if (!prev) return null;
         const novaBateria = Math.max(0, prev.bateria - 0.5);
         const novoCombustivel = Math.max(0, prev.combustivel - 0.2);

         const podeOperar =
            novaBateria > 0 && novoCombustivel > 0 && prev.integridade > 0;

         // (Verificação de derrota agora é centralizada no 'handleAction')

         return {
            ...prev,
            bateria: novaBateria,
            combustivel: novoCombustivel,
            podeOperar: podeOperar,
         };
      });

      return droneStats;
   };

   // 6. FUNÇÃO DE INICIALIZAÇÃO (SIMULADA - LOGS LIMPOS)
   useEffect(() => {
      const iniciarMissao = async () => {
         setIsLoading(true);
         setError(null);
         setCombatLog(["Iniciando conexão..."]);
         setGameStatus("carregando");

         try {
            // 1. SIMULAR O DRONE
            setCombatLog((prev) => [
               ...prev.slice(-100),
               `Solicitando drone para o alvo ${patoId}...`,
            ]);

            const initialDroneData = formatarDroneData({
               numeroSerie: "DRONE-SIM-001",
               bateriaPorcentagem: 100,
               combustivelPorcentagem: 100,
               integridadePorcentagem: 100,
               latitude: -23.5505,
               longitude: -46.6333,
               podeOperar: true,
            });

            setDroneStats(initialDroneData);
            setCombatLog((prev) => [
               ...prev.slice(-100),
               `Drone ${initialDroneData.numeroSerie} alocado.`,
            ]);

            // 2. BUSCAR DADOS DO PATO
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

            // 3. INICIAR O JOGO
            setGameStatus("em_andamento");
         } catch (err) {
            console.error("Falha ao buscar dados do pato", err);
            const errorMsg = err.response
               ? JSON.stringify(err.response.data)
               : err.message;
            setCombatLog((prev) => [
               ...prev.slice(-100),
               `ERRO CRÍTICO AO BUSCAR DADOS DO ALVO: ${errorMsg}`,
            ]);
            setError(err.message || "Falha ao buscar dados iniciais do Pato.");
            setGameStatus("erro");
         } finally {
            setIsLoading(false);
         }
      };

      iniciarMissao();
   }, [patoId]);

   // 7. FUNÇÃO DE AÇÕES (SIMULADA - CORRIGIDA VITÓRIA/DERROTA)
   const handleAction = async (acaoId) => {
      if (isProcessing || gameStatus !== "em_andamento" || !droneStats) return;
      setIsProcessing(true);
      setError(null);

      const acao = ACOES_DISPONIVEIS.find((a) => a.id === acaoId);
      const acaoNome = acao?.nome || acaoId;
      setCombatLog((prev) => [
         ...prev.slice(-100),
         `Executando: ${acaoNome}... (Drone: ${droneStats.numeroSerie})`,
      ]);

      let logAcaoEspecifica = [];
      let gastoBateria = 2;
      let gastoCombustivel = 1;
      let danoDrone = 0; // Dano que o drone vai sofrer
      let patoMorreu = false; // Flag de vitória

      try {
         if (acaoId === "mover") {
            gastoBateria = 5;
            gastoCombustivel = 3;
            const novaLat = (droneStats?.latitude || 0) + 0.001;
            const novaLon = (droneStats?.longitude || 0) + 0.001;

            setDroneStats((prev) => ({
               ...prev,
               latitude: novaLat,
               longitude: novaLon,
            }));
            logAcaoEspecifica.push(
               `Movimento simulado para (${novaLat.toFixed(
                  4
               )}, ${novaLon.toFixed(4)}).`
            );
         } else if (acaoId === "scan_fraqueza") {
            gastoBateria = 4;
            gastoCombustivel = 0.5;
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
            logAcaoEspecifica.push("--------------------------");
         } else {
            // --- AÇÕES DE COMBATE ---
            gastoBateria = 3;
            gastoCombustivel = 1;

            const resultadoTurnoPato = simularTurnoCombate(acaoId, patoStats);
            logAcaoEspecifica.push(...resultadoTurnoPato.logEntries);

            setPatoStats(resultadoTurnoPato.novoPatoStats);

            const logContemAlerta = resultadoTurnoPato.logEntries.some((log) =>
               log.startsWith("ALERTA!")
            );

            if (logContemAlerta && patoStats.status === "Desperto") {
               const danoSimulado = Math.floor(Math.random() * 15) + 5;
               danoDrone = Math.max(1, (danoSimulado / 100) * 100);
               logAcaoEspecifica.push(
                  `ALERTA: Drone sofreu ${danoDrone.toFixed(0)}% de dano!`
               );
            }

            if (resultadoTurnoPato.status === "vitoria") {
               patoMorreu = true;
            }
         }

         setCombatLog((prev) => [...prev.slice(-100), ...logAcaoEspecifica]);
      } catch (err) {
         console.error(`Erro ao simular ação ${acaoId}:`, err);
         const errorMsg = `>> Falha ao simular ${acaoNome}. <<`;
         setCombatLog((prev) => [...prev.slice(-100), errorMsg]);
         setError(`Falha ao simular ${acaoNome}.`);
      } finally {
         // --- BLOCO DE VERIFICAÇÃO FINAL ---

         // 'gameStatus' aqui é o estado ANTES do 'setGameStatus'
         const statusAtual = gameStatus;

         setDroneStats((prev) => {
            const novoDrone = { ...prev };
            novoDrone.bateria = Math.max(0, prev.bateria - gastoBateria);
            novoDrone.combustivel = Math.max(
               0,
               prev.combustivel - gastoCombustivel
            );
            novoDrone.integridade = Math.max(0, prev.integridade - danoDrone);

            const podeOperar =
               novoDrone.bateria > 0 &&
               novoDrone.combustivel > 0 &&
               novoDrone.integridade > 0;
            novoDrone.podeOperar = podeOperar;

            // Verifica o estado DO JOGO
            if (patoMorreu && statusAtual === "em_andamento") {
               setGameStatus("vitoria");
            } else if (
               !podeOperar &&
               statusAtual === "em_andamento" &&
               !patoMorreu
            ) {
               setGameStatus("derrota");
            }

            return novoDrone;
         });

         setIsProcessing(false);
      }
   };

   // 8. RENDERIZAÇÃO (JSX)

   if (isLoading) {
      return (
         <div className={styles.loadingContainer}>
            <h1>Carregando Missão...</h1>
         </div>
      );
   }

   if (error && gameStatus === "erro") {
      return (
         <div className={styles.loadingContainer}>
            <h1 style={{ color: "var(--color-danger)" }}>ERRO NA MISSÃO</h1>
            <p>{error}</p>
            <button onClick={() => navigate("/dashboard/patopedia")}>
               Voltar
            </button>
         </div>
      );
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
            <div className={styles.endGameOverlay}>
               <h1 className={styles.vitoria}>ALVO NEUTRALIZADO</h1>
               <p>Missão concluída com sucesso!</p>
               <div className={styles.endGameButtons}>
                  <button onClick={() => navigate("/dashboard/patopedia")}>
                     Voltar
                  </button>
               </div>
            </div>
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
                  <button onClick={() => navigate("/dashboard/patopedia")}>
                     Voltar
                  </button>
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
               <h2>DRONE: {droneStats.numeroSerie}</h2>
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
            {/* Botão Tentar Novamente (visível em erro/fim) */}
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
