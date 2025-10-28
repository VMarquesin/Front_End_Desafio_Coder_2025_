import { useState, useEffect } from "react";
import { useParams, Link } from "react-router-dom";
import api from "../services/api";
import styles from "./PatoDetailPage.module.css";

const getStatusClass = (status) => {
   switch (status) {
      case "desperto":
         return styles.statusDesperto;
      case "em transe":
         return styles.statusEmTranse;
      case "hibernacao profunda":
         return styles.statusHibernacao;
      default:
         return "";
   }
};

const PatoDetailPage = () => {
   const { id } = useParams();
   const [pato, setPato] = useState(null);
   const [isLoading, setIsLoading] = useState(true);
   const [error, setError] = useState(null);

   useEffect(() => {
      const carregarDetalhes = async () => {
         setIsLoading(true);
         setError(null);
         try {
            const response = await api.get(`/patos/${id}`);
            setPato(response.data);
         } catch (err) {
            console.error("Falha ao carregar detalhes do Pato", err);
            if (err.response && err.response.status === 404) {
               setError(`Arquivo do Pato ID: ${id} não encontrado.`);
            } else {
               setError("Falha ao carregar dados do pato.");
            }
            setPato(null);
         } finally {
            setIsLoading(false);
         }
      };
      carregarDetalhes();
   }, [id]);

   if (isLoading) {
      return (
         <div className={styles.loadingContainer}>
            <h1>Carregando Arquivo...</h1>
            <p>Acessando banco de dados DSIN...</p>
         </div>
      );
   }

   if (!pato) {
      return (
         <div className={styles.loadingContainer}>
            <h1 style={{ color: "var(--color-danger)" }}>Erro 404</h1>
            <p>Arquivo do Pato ID:{id} não encontrado.</p>
         </div>
      );
   }

   return (
      <div className={styles.detailContainer}>
         <div className={styles.header}>
            <h1>Arquivo do Pato: {pato.pontoReferencia || `ID ${pato.id}`}</h1>
            <p className={styles.subheading}>
               Análise detalhada, sequência de DNA e opção de captura.
            </p>
         </div>

         <div className={styles.contentGrid}>
            {/* Box 1: Informações Físicas */}
            <div className={styles.infoBox}>
               <h2>Relatório Físico</h2>
               <ul className={styles.infoList}>
                  <li className={styles.infoItem}>
                     <span className={styles.infoLabel}>Status</span>
                     <span
                        className={`${styles.infoValue} ${getStatusClass(
                           pato.status
                        )}`}
                     >
                        {pato.status}
                     </span>
                  </li>
                  <li className={styles.infoItem}>
                     <span className={styles.infoLabel}>Altura</span>
                     <span className={styles.infoValue}>
                        {pato.alturaValor?.toFixed(1)} {pato.alturaUnidade}
                     </span>
                  </li>
                  <li className={styles.infoItem}>
                     <span className={styles.infoLabel}>Peso</span>
                     <span className={styles.infoValue}>
                        {(
                           pato.pesoValor /
                           (pato.pesoUnidade === "Grama" ? 1000 : 1)
                        )?.toFixed(2)}{" "}
                        kg
                     </span>
                  </li>
                  <li className={styles.infoItem}>
                     <span className={styles.infoLabel}>Batimentos</span>
                     <span className={styles.infoValue}>
                        {pato.batimentosPorMinuto || "N/A"} bpm
                     </span>
                  </li>
               </ul>
            </div>
            {/* Box 2: Relatório de Risco - VERIFIQUE SE A API RETORNA ISSO */}
            {pato.relatorioRisco && (
               <div className={styles.infoBox}>
                  <h2>Análise de Risco (Missão 2)</h2>
                  <ul className={styles.infoList}>
                     <li className={styles.infoItem}>
                        <span className={styles.infoLabel}>
                           Score Risco Captura
                        </span>
                        <span
                           className={`${styles.infoValue} ${styles.statusDesperto}`}
                        >
                           {pato.relatorioRisco.captureRiskScore?.toFixed(1)}%
                        </span>
                     </li>
                     <li className={styles.infoItem}>
                        <span className={styles.infoLabel}>
                           Score Custo Operacional
                        </span>
                        <span className={styles.infoValue}>
                           {pato.relatorioRisco.operationalCostScore?.toFixed(
                              1
                           )}
                           %
                        </span>
                     </li>
                     <li className={styles.infoItem}>
                        <span className={styles.infoLabel}>
                           Score Valor Científico
                        </span>
                        <span className={styles.infoValue}>
                           {pato.relatorioRisco.scientificValueScore?.toFixed(
                              1
                           )}
                           %
                        </span>
                     </li>
                     <li className={styles.infoItem}>
                        <span className={styles.infoLabel}>
                           Força Necessária
                        </span>
                        <span className={styles.infoValue}>
                           {pato.relatorioRisco.requiredForceLevel}
                        </span>
                     </li>
                     <li className={styles.infoItem}>
                        <span className={styles.infoLabel}>Recomendação</span>
                        <span className={styles.infoValue}>
                           {pato.relatorioRisco.recommendation}
                        </span>
                     </li>
                     <li className={styles.infoItem}>
                        <span className={styles.infoLabel}>
                           Custo Transporte Est.
                        </span>
                        <span className={styles.infoValue}>
                           R${" "}
                           {pato.relatorioRisco.estimatedTransportCost?.toFixed(
                              2
                           )}
                        </span>
                     </li>
                  </ul>
               </div>
            )}

            {/* Box 3: Super-poder (Use os nomes da API) */}
            {pato.poderNome && (
               <div className={`${styles.infoBox} ${styles.dnaBox}`}>
                  <h2>Habilidade Primordial Detectada</h2>
                  <ul className={styles.infoList}>
                     <li className={styles.infoItem}>
                        <span className={styles.infoLabel}>Nome</span>
                        <span className={styles.infoValue}>
                           {pato.poderNome}
                        </span>
                     </li>
                     <li className={styles.infoItem}>
                        <span className={styles.infoLabel}>Descrição</span>
                        <span className={styles.infoValue}>
                           {pato.poderDescricao}
                        </span>
                     </li>
                     <li className={styles.infoItem}>
                        <span className={styles.infoLabel}>Classificação</span>
                        <span className={styles.infoValue}>
                           {pato.poderClassificacao}
                        </span>
                     </li>
                  </ul>
               </div>
            )}

            {/* Box 4: Sequência de DNA */}
            {pato.sequenciaDna && (
               <div className={`${styles.infoBox} ${styles.dnaBox}`}>
                  <h2>
                     Sequenciamento Genético ({pato.quantidadeMutacoes}{" "}
                     mutações)
                  </h2>
                  <div className={styles.dnaSequence}>
                     {pato.sequenciaDna
                        .split(/(\[.*?\])/)
                        .map((part, index) =>
                           part.startsWith("[") ? (
                              <span key={index}>{part}</span>
                           ) : (
                              part
                           )
                        )}
                  </div>
               </div>
            )}
         </div>

         {/* BOTÃO MISSÃO DE CAPTURA */}
         {pato.status === "Desperto" && (
            <Link
               to={`/dashboard/missao/${pato.id}`}
               className={styles.missionButton}
            >
               INICIAR MISSÃO DE CAPTURA
            </Link>
         )}
         {pato.status !== "Desperto" && (
            <Link
               to={`/dashboard/missao/${pato.id}`}
               className={styles.missionButton}
               style={{ backgroundColor: "var(--color-accent)" }}
            >
               INICIAR MISSÃO DE EXTRAÇÃO (Segura)
            </Link>
         )}
      </div>
   );
};

export default PatoDetailPage;
