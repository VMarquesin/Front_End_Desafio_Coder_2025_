import { Link } from 'react-router-dom'
import styles from './PatoCard.module.css'

const PatoIcon = () => (
  <svg className={styles.patoIcon} viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
    <path d="M15.5 14C17.433 14 19 12.433 19 10.5C19 8.567 17.433 7 15.5 7C13.567 7 12 8.567 12 10.5C12 12.433 13.567 14 15.5 14Z" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
    <path d="M7 20C7 17.2386 9.23858 15 12 15C12.5929 15 13.1652 15.101 13.7084 15.2892M9 12C9 14.2091 7.20914 16 5 16C2.79086 16 1 14.2091 1 12C1 9.79086 2.79086 8 5 8C7.20914 8 9 9.79086 9 12Z" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
    <path d="M19.333 19.333L22 22" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
  </svg>
)

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

const PatoCard = ({ pato }) => {
  const statusClassName = getStatusClass(pato.status_hibernacao)
  
  return (
    <div className={styles.card}>
      <div className={styles.cardImagePlaceholder}>
        <PatoIcon />
      </div>
      <div className={styles.cardContent}>
        <span className={`${styles.statusIndicator} ${statusClassName}`}>
          {pato.status_hibernacao}
        </span>
        
        <h3 className={styles.cardTitle}>
          {pato.ponto_referencia || `Pato ID: ${pato.id}`}
        </h3>
        
        <p className={styles.cardInfo}>
          Localização: {pato.cidade || 'Desconhecida'}, {pato.pais || 'Desconhecido'}
        </p>
        <p className={styles.cardInfo}>
          Mutações: {pato.quantidade_mutacoes || 0}
        </p>
        
        <Link to={`/pato/${pato.id}`} className={styles.detailsLink}>
          Analisar Sequência de DNA 
        </Link>
      </div>
    </div>
  )
}

export default PatoCard