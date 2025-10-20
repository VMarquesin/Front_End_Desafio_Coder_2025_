import { useState, useEffect } from 'react'
import { useParams, Link } from 'react-router-dom'
import api from '../services/api'
import styles from './PatoDetailPage.module.css'

// --- DADOS MOCKADOS ---
// 'GET /api/patos/{id}'
const MOCK_PATO_DETAIL = {
  id: 1,
  drone_serial: "BR-001",
  ponto_referencia: "Pico da Neblina",
  cidade: "S. Isabel do Rio Negro",
  pais: "Brasil",
  latitude: -0.413,
  longitude: -65.019,
  altura_cm: 210.5,
  peso_g: 120000,
  status_hibernacao: "desperto",
  batimentos_cardiacos_bpm: null, // Nulo por estar desperto
  quantidade_mutacoes: 88,
  
  // backend gerar fator DNA
  sequencia_dna: "TTA-GGC-CGA-[MUT-01:ASAS-DE-ACO]-GGC-ATT...[MUT-87:OLHOS-LASER]-CCC-TTG-GAA...[MUT-88:TEMPESTADE-ELETRICA]-GGC-ATT-TTA-GGC-CGA-GGC-ATT-TTA-GGC-CGA-GGC-ATT-TTA-GGC-CGA-GGC-ATT-TTA-GGC-CGA-GGC-ATT",

  // Dados do Relatório de Risco
  relatorio_risco: {
    score_custo_beneficio: 0.005,
    grau_risco_percentual: 100,
    poderio_militar_necessario: "Extremo (Nível Vingadores)",
    custo_operacional_estimado: 50000,
    ganho_conhecimento_estimado: 1080
  },
  
  // Dados do Super-Poder
  super_poder: {
    nome: "Tempestade Elétrica",
    descricao: "Gera descargas elétricas em área.",
    classificacao: "bélico, raro, alto risco"
  }
};
// ******************

// Função helper para definir a cor do status
const getStatusClass = (status) => {
  switch (status) {
    case 'desperto':
      return styles.statusDesperto
    case 'em transe':
      return styles.statusEmTranse
    case 'hibernacao profunda':
      return styles.statusHibernacao
    default:
      return ''
  }
}

const PatoDetailPage = () => {
  const { id } = useParams()
  const [pato, setPato] = useState(null)
  const [isLoading, setIsLoading] = useState(true)

  useEffect(() => {
    const carregarDetalhes = async () => {
      try {
        // const response = await api.get(`/patos/${id}`)
        // setPato(response.data)
        
        // --- MOCK ---
        setPato(MOCK_PATO_DETAIL)

      } catch (err) {
        console.error("Falha ao carregar detalhes do Pato", err)
      } finally {
        setIsLoading(false)
      }
    }

    setTimeout(carregarDetalhes, 500) // delay
  }, [id])

  if (isLoading) {
    return (
      <div className={styles.loadingContainer}>
        <h1>Carregando Arquivo...</h1>
        <p>Acessando banco de dados DSIN...</p>
      </div>
    )
  }

  if (!pato) {
    return (
      <div className={styles.loadingContainer}>
        <h1 style={{ color: 'var(--color-danger)'}}>Erro 404</h1>
        <p>Arquivo do Pato ID:{id} não encontrado.</p>
      </div>
    )
  }

  // A tela de detalhes completa
  return (
    <div className={styles.detailContainer}>
      <div className={styles.header}>
        <h1>Arquivo do Pato: {pato.ponto_referencia || `ID ${pato.id}`}</h1>
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
              <span className={`${styles.infoValue} ${getStatusClass(pato.status_hibernacao)}`}>
                {pato.status_hibernacao}
              </span>
            </li>
            <li className={styles.infoItem}>
              <span className={styles.infoLabel}>Altura</span>
              <span className={styles.infoValue}>{pato.altura_cm.toFixed(1)} cm</span>
            </li>
            <li className={styles.infoItem}>
              <span className={styles.infoLabel}>Peso</span>
              <span className={styles.infoValue}>{(pato.peso_g / 1000).toFixed(2)} kg</span>
            </li>
            <li className={styles.infoItem}>
              <span className={styles.infoLabel}>Batimentos</span>
              <span className={styles.infoValue}>{pato.batimentos_cardiacos_bpm || 'N/A'} bpm</span>
            </li>
          </ul>
        </div>
        
        {/* Box 2: Relatório de Risco */}
        <div className={styles.infoBox}>
          <h2>Análise de Risco (Missão 2)</h2>
          <ul className={styles.infoList}>
            <li className={styles.infoItem}>
              <span className={styles.infoLabel}>Nível de Risco</span>
              <span className={`${styles.infoValue} ${styles.statusDesperto}`}>
                {pato.relatorio_risco.grau_risco_percentual}%
              </span>
            </li>
            <li className={styles.infoItem}>
              <span className={styles.infoLabel}>Poderio Necessário</span>
              <span className={styles.infoValue}>{pato.relatorio_risco.poderio_militar_necessario}</span>
            </li>
            <li className={styles.infoItem}>
              <span className={styles.infoLabel}>Ganho Científico</span>
              <span className={styles.infoValue}>{pato.relatorio_risco.ganho_conhecimento_estimado}</span>
            </li>
            <li className={styles.infoItem}>
              <span className={styles.infoLabel}>Score C/B</span>
              <span className={styles.infoValue}>{pato.relatorio_risco.score_custo_beneficio.toFixed(4)}</span>
            </li>
          </ul>
        </div>

        {/* Box 3: Super-poder */}
        {pato.super_poder && (
          <div className={`${styles.infoBox} ${styles.dnaBox}`}>
            <h2>Habilidade Primordial Detectada</h2>
            <ul className={styles.infoList}>
              <li className={styles.infoItem}>
                <span className={styles.infoLabel}>Nome</span>
                <span className={styles.infoValue}>{pato.super_poder.nome}</span>
              </li>
              <li className={styles.infoItem}>
                <span className={styles.infoLabel}>Descrição</span>
                <span className={styles.infoValue}>{pato.super_poder.descricao}</span>
              </li>
              <li className={styles.infoItem}>
                <span className={styles.infoLabel}>Classificação</span>
                <span className={styles.infoValue}>{pato.super_poder.classificacao}</span>
              </li>
            </ul>
          </div>
        )}

        {/* Box 4: Sequência de DNA */}
        <div className={`${styles.infoBox} ${styles.dnaBox}`}>
          <h2>Sequenciamento Genético ({pato.quantidade_mutacoes} mutações)</h2>
          <div className={styles.dnaSequence}>
            {/* Lógica para destacar as mutações */}
            {pato.sequencia_dna.split(/(\[.*?\])/).map((part, index) => 
              part.startsWith('[') ? 
              <span key={index}>{part}</span> : 
              part
            )}
          </div>
        </div>
      </div>
      
      {/* O BOTÃO MISSÃO DE CAPTURA */}
      {pato.status_hibernacao === 'desperto' && (
         <Link to={`/missao/${pato.id}`} className={styles.missionButton}>
           INICIAR MISSÃO DE CAPTURA
         </Link>
      )}
      {pato.status_hibernacao !== 'desperto' && (
         <Link to={`/missao/${pato.id}`} className={styles.missionButton} style={{ backgroundColor: 'var(--color-accent)'}}>
           INICIAR MISSÃO DE EXTRAÇÃO (Segura)
         </Link>
      )}
      
    </div>
  )
}

export default PatoDetailPage