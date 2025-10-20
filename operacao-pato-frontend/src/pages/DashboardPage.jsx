import { useState } from 'react'
import PatoMap from '../components/PatoMap'
import CatalogModal from '../components/CatalogModal'
import styles from './DashboardPage.module.css'

const DashboardPage = () => {
  // abertura do modal
  const [isModalOpen, setIsModalOpen] = useState(false)
  
  // Este é um truque para forçar o PatoMap a recarregar
  // Quando o 'key' de um componente muda, o React o recria do zero
  const [mapKey, setMapKey] = useState(Date.now())

  const handlePatoCatalogado = () => {
    setIsModalOpen(false)
    // 2. Mude a 'key' do PatoMap para forçar o recarregamento dos dados
    setMapKey(Date.now()) 
  }

  return (
    <div className={styles.dashboardContainer}>
      {isModalOpen && (
        <CatalogModal 
          onClose={() => setIsModalOpen(false)} 
          onPatoCatalogado={handlePatoCatalogado}
        />
      )}

      <div className={styles.header}>
        <div>
          <h1>Dashboard Global de Ameaças</h1>
          <p className={styles.subheading}>
            Visualize todos os Patos Primordiais catalogados em tempo real.
          </p>
        </div>
        <button 
          className={styles.catalogButton} 
          onClick={() => setIsModalOpen(true)}
        >
          Catalogar Novo Pato
        </button>
      </div>
      
      <div className={styles.mapWrapper}>
        {/* 4. Passe a 'key' para o PatoMap */}
        <PatoMap key={mapKey} />
      </div>
    </div>
  )
}

export default DashboardPage