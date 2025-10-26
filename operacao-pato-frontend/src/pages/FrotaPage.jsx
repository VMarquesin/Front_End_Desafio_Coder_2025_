import { useState, useEffect } from 'react'
import api from '../services/api'
import styles from './FrotaPage.module.css'
import DroneModal from '../components/DroneModal'


const FrotaPage = () => {
  const [drones, setDrones] = useState([])
  const [isLoading, setIsLoading] = useState(true)
  const [isModalOpen, setIsModalOpen] = useState(false)
  const carregarDrones = async () => {
    setIsLoading(true); 
    try {
      const listDrones = (await api.get('/drones')).data;
      console.log(listDrones)
      setDrones(listDrones)
     
    } catch (err) {
      console.error("Falha ao carregar frota de drones", err)
    } finally {
      setIsLoading(false)
    }
  }
  useEffect(() => {
    carregarDrones()
  }, []) 

  const handleDroneCadastrado = () => {
    setIsModalOpen(false); 
    carregarDrones(); 
  }

  return (
    <div>
      {isModalOpen && (
        <DroneModal 
          onClose={() => setIsModalOpen(false)}
          onDroneCadastrado={handleDroneCadastrado}
        />
      )}

      <div className={styles.header}>
        <h1>Gerenciador de Frota de Drones</h1>
      
        <button 
          className={styles.registerButton}
          onClick={() => setIsModalOpen(true)} 
        >
          Cadastrar Novo Drone
        </button>
      </div>
      <p style={{ color: 'var(--color-text-secondary)', marginBottom: '24px' }}>
        Visualize e gerencie todos os drones de reconhecimento ativos.
      </p>

      <table className={styles.frotaTable}>
        <thead>
          <tr>
            <th>Nº de Série</th>
            <th>Marca</th>
            <th>Fabricante</th>
            <th>País de Origem</th>
          </tr>
        </thead>
        <tbody>
          {isLoading ? (
            <tr>
              <td colSpan="4" className={styles.loadingText}>Carregando frota...</td>
            </tr>
          ) : (
            drones.map(drone => (
              <tr key={drone.numeroSerie}>
                <td className={styles.serial}>{drone.numeroSerie}</td>
                <td>{drone.marca}</td>
                <td>{drone.fabricante}</td>
                <td>{drone.paisOrigem}</td>
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  )
}

export default FrotaPage