import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import api from "../services/api";
import StatusBar from "../components/StatusBar";
import CombatLog from "../components/CombatLog";
import styles from "./MissionControlPage.module.css";

// --- DADOS MOCKADOS ---
const MOCK_INITIAL_STATE = {
   droneStats: {
      integridade: 100,
      integridade_max: 100,
      bateria: 100,
      bateria_max: 100,
   },
   patoStats: {
      id: 1,
      nome: "Pato do Pico da Neblina",
      integridade: 150,
      integridade_max: 150,
      super_poder: "Tempestade Elétrica",
   },
   acoesDisponiveis: [
      { id: "ataque_padrao", nome: "Ataque Padrão (Plano A)" },
      { id: "ataque_pedra", nome: "Ataque Aéreo (Plano B)" },
      { id: "scan_fraqueza", nome: "Escanear Pontos Fracos" },
   ],
};

// const MOCK_API_RESPONSE = {
//    droneStats: {
//       integridade: 85,
//       integridade_max: 100,
//       bateria: 95,
//       bateria_max: 100,
//    },
//    patoStats: {
//       id: 1,
//       nome: "Pato do Pico da Neblina",
//       integridade: 110,
//       integridade_max: 150,
//       super_poder: "Tempestade Elétrica",
//    },
//    logEntries: [
//       "Drone usou 'Ataque Aéreo (Plano A)'. Dano: 40.",
//       "ALERTA! Pato usou 'Tempestade Elétrica'! Dano ao Drone: 15.",
//    ],
//    status: "em_andamento", // ou 'vitoria', 'derrota'
// };
// ---------------------------------
// --- SISTEMA DE COMBATE LOCAL (simula o backend) ---
const gerarRespostaDeCombate = (acaoId, estadoAtual) => {
   const { droneStats, patoStats } = estadoAtual;
   const log = [];

   let novoDrone = { ...droneStats };
   let novoPato = { ...patoStats };

   // 🔹 1. Drone executa a ação
   switch (acaoId) {
      case "ataque_padrao":
         const danoPadrao = Math.floor(Math.random() * 15) + 10;
         novoPato.integridade = Math.max(0, novoPato.integridade - danoPadrao);
         novoDrone.bateria = Math.max(0, novoDrone.bateria - 5);
         log.push(
            `Drone realizou um ataque padrão causando ${danoPadrao} de dano.`
         );
         break;

      case "ataque_pedra":
         const danoAereo =
            novoPato.integridade_max > 100
               ? Math.floor(Math.random() * 30) + 20
               : Math.floor(Math.random() * 10) + 5;
         novoPato.integridade = Math.max(0, novoPato.integridade - danoAereo);
         novoDrone.bateria = Math.max(0, novoDrone.bateria - 10);
         log.push(
            `Drone lançou um ataque aéreo causando ${danoAereo} de dano!`
         );
         break;

      case "scan_fraqueza":
         const fraquezas = [
            "Chocolate",
            "Ruído metálico",
            "Ondas sonoras agudas",
            "Efeito espelho",
            "Presença de crianças em festas",
         ];
         const fraqueza =
            fraquezas[Math.floor(Math.random() * fraquezas.length)];
         log.push(
            `Escaneando fraquezas... Detecção completa: fraqueza encontrada — ${fraqueza}.`
         );
         novoDrone.bateria = Math.max(0, novoDrone.bateria - 15);
         break;

      default:
         log.push("Ação desconhecida executada...");
   }

   // 🔹 2. Pato contra-ataca (se ainda estiver vivo)
   if (novoPato.integridade > 0) {
      const danoPato = Math.floor(Math.random() * 20) + 5;
      novoDrone.integridade = Math.max(0, novoDrone.integridade - danoPato);
      log.push(
         `ALERTA! ${novoPato.nome} usou '${novoPato.super_poder}' e causou ${danoPato} de dano!`
      );
   }

   // 🔹 3. Sistema de defesa aleatória do drone
   if (Math.random() < 0.2) {
      const defesas = [
         "Gerou escudo eletromagnético!",
         "Ativou drones-iscas holográficos!",
         "Teleportou crianças com brigadeiros — distração efetiva!",
         "Soltou fumaça de banana — pato confuso!",
         "Emitiu sinal de acasalamento de pato robótico!",
      ];
      const defesa = defesas[Math.floor(Math.random() * defesas.length)];
      log.push(`🌀 Defesa aleatória ativada: ${defesa}`);
   }

   // 🔹 4. Verificar status final
   let status = "em_andamento";
   if (novoPato.integridade <= 0) status = "vitoria";
   else if (novoDrone.integridade <= 0) status = "derrota";

   return {
      droneStats: novoDrone,
      patoStats: novoPato,
      logEntries: log,
      status,
   };
};

const MissionControlPage = () => {
   const { patoId } = useParams();
   const navigate = useNavigate();

   // Estados do Jogo
   const [droneStats, setDroneStats] = useState(null);
   const [patoStats, setPatoStats] = useState(null);
   const [acoes, setAcoes] = useState([]);
   const [combatLog, setCombatLog] = useState([
      "Iniciando conexão com o drone de captura...",
   ]);
   const [gameStatus, setGameStatus] = useState("carregando"); // carregando, em_andamento, vitoria, derrota
   const [isProcessing, setIsProcessing] = useState(false); // Para desabilitar botões

   // Carregar dados iniciais do combate
   useEffect(() => {
      const iniciarMissao = async () => {
         try {
            // --- QUANDO A API ESTIVER PRONTA ---
            // const response = await api.get(`/combate/${patoId}/iniciar`)
            // setDroneStats(response.data.droneStats)
            // setPatoStats(response.data.patoStats)
            // setAcoes(response.data.acoesDisponiveis)
            // setCombatLog(prev => [...prev, "Conexão estabelecida. Alvo na mira."])

            // --- MOCK ---
            setDroneStats(MOCK_INITIAL_STATE.droneStats);
            setPatoStats(MOCK_INITIAL_STATE.patoStats);
            setAcoes(MOCK_INITIAL_STATE.acoesDisponiveis);
            setCombatLog((prev) => [
               ...prev,
               `Alvo ID ${patoId} na mira. ${MOCK_INITIAL_STATE.patoStats.nome}.`,
            ]);

            setGameStatus("em_andamento");
         } catch (err) {
            console.error("Falha ao iniciar missão", err);
            setCombatLog((prev) => [
               ...prev,
               "ERRO CRÍTICO: Falha ao conectar com o drone.",
            ]);
            setGameStatus("derrota");
         }
      };

      setTimeout(iniciarMissao, 1000); // delay
   }, [patoId]);

   // Função para enviar uma ação para o backend
   const handleAction = async (acaoId) => {
      if (isProcessing || gameStatus !== "em_andamento") return;

      setIsProcessing(true);
      setCombatLog((prev) => [...prev, `Executando ação: ${acaoId}...`]);

      try {
         // --- QUANDO A API ESTIVER PRONTA ---
         // const response = await api.post(`/combate/${patoId}/acao`, { acao: acaoId })

         // --- MOCK ---
         const response = {
            data: gerarRespostaDeCombate(acaoId, { droneStats, patoStats }),
         };
         // Simular um delay da API
         await new Promise((res) => setTimeout(res, 700));

         // Atualizar o estado do jogo com a resposta da API
         setDroneStats(response.data.droneStats);
         setPatoStats(response.data.patoStats);
         setCombatLog((prev) => [...prev, ...response.data.logEntries]);
         setGameStatus(response.data.status);

         if (response.data.status === "vitoria") {
            setCombatLog((prev) => [
               ...prev,
               "ALVO NEUTRALIZADO. Captura bem-sucedida!",
            ]);
         } else if (response.data.status === "derrota") {
            setCombatLog((prev) => [
               ...prev,
               "SINAL DO DRONE PERDIDO. Missão falhou.",
            ]);
         }
      } catch (err) {
         console.error("Erro na ação de combate", err);
         setCombatLog((prev) => [...prev, "Falha na comunicação com o drone."]);
      } finally {
         setIsProcessing(false);
      }
   };

   // placeholder
   if (gameStatus === "carregando" || !droneStats || !patoStats) {
      return (
         <div className={styles.loadingContainer}>
            <h1>Carregando Mission Control...</h1>
            <p>Estabelecendo link com o drone de captura...</p>
         </div>
      );
   }

   // Tela principal do combate
   return (
      <div className={styles.missionContainer}>
         {/* Pop-up de Fim de Jogo */}
         {gameStatus === "vitoria" && (
            <div className={styles.endGameOverlay}>
               <h1>VITÓRIA</h1>
               <p>Alvo capturado com sucesso!</p>
               <button onClick={() => navigate("/patopedia")}>
                  Voltar para Pato-pédia
               </button>
            </div>
         )}
         {gameStatus === "derrota" && (
            <div className={styles.endGameOverlay}>
               <h1 className={styles.derrota}>MISSÃO FALHOU</h1>
               <p>O drone foi destruído.</p>
               <button onClick={() => navigate("/patopedia")}>
                  Voltar para Pato-pédia
               </button>
            </div>
         )}

         {/* Painéis de Status (Esquerda e Direita) */}
         <div className={styles.statusPanels}>
            <div className={styles.panel}>
               <h2>DRONE DE CAPTURA</h2>
               <StatusBar
                  label="Integridade"
                  current={droneStats.integridade}
                  max={droneStats.integridade_max}
                  color="var(--color-accent)"
               />
               <StatusBar
                  label="Bateria"
                  current={droneStats.bateria}
                  max={droneStats.bateria_max}
                  color="#3b82f6"
               />
            </div>

            <div className={styles.panel}>
               <h2>ALVO: {patoStats.nome}</h2>
               <StatusBar
                  label="Integridade do Alvo"
                  current={patoStats.integridade}
                  max={patoStats.integridade_max}
                  color="var(--color-danger)"
               />
               <p className={styles.patoInfo}>
                  Super-poder detectado: <span>{patoStats.super_poder}</span>
               </p>
            </div>
         </div>

         {/* Painel Central (Log) */}
         <div className={styles.logPanel}>
            <CombatLog logs={combatLog} />
         </div>

         {/* Painel Inferior (Ações) */}
         <div className={styles.actionBar}>
            {/* 1. Crie uma div para agrupar as ações principais */}
            <div className={styles.mainActions}>
               {acoes.map((acao) => (
                  <button
                     key={acao.id}
                     className={styles.actionButton}
                     onClick={() => handleAction(acao.id)}
                     disabled={isProcessing || gameStatus !== "em_andamento"}
                  >
                     {acao.nome}
                  </button>
               ))}
            </div>

            {/* 2. Adicione uma className ao seu novo botão */}
            <button
               className={styles.retryButton}
               onClick={() => {
                  setDroneStats(MOCK_INITIAL_STATE.droneStats);
                  setPatoStats(MOCK_INITIAL_STATE.patoStats);
                  setCombatLog(["Iniciando nova missão..."]);
                  setGameStatus("em_andamento");
               }}
            >
               Tentar novamente
            </button>
         </div>
      </div>
   );
};

export default MissionControlPage;
