import { useState } from 'react'
import PatoMap from '../components/PatoMap'
import CatalogModal from '../components/CatalogModal'
import styles from './DashboardPage.module.css'

const DashboardPage = () => {
  const [isModalOpen, setIsModalOpen] = useState(false)
  const [mapKey, setMapKey] = useState(Date.now())
  const handlePatoCatalogado = () => {
    setIsModalOpen(false)
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
        <PatoMap key={mapKey} />
      </div>
    </div>
  )
}

export default DashboardPage