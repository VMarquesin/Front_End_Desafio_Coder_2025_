import { useState, useEffect } from 'react'
import api from '../services/api'
import styles from './FrotaPage.module.css'
import DroneModal from '../components/DroneModal'

// ... (MOCK_DRONES continua igual)
const MOCK_DRONES = [
  { serial_number: "BR-001", marca: "Carcará", fabricante: "Embraer Defesa", pais_origem: "Brasil" },
  { serial_number: "USA-007", marca: "SkyHawk", fabricante: "US Drones Inc.", pais_origem: "EUA" },
  { serial_number: "GER-003", marca: "Kondor", fabricante: "Luftwaffe Systems", pais_origem: "Alemanha" },
];

const FrotaPage = () => {
  const [drones, setDrones] = useState([])
  const [isLoading, setIsLoading] = useState(true)
  
  // 2. STATE PARA CONTROLAR O MODAL
  const [isModalOpen, setIsModalOpen] = useState(false)

  // 3. REFATORAR A LÓGICA DE CARREGAMENTO
  // Colocamos a lógica de carregar drones em uma função
  // para que ela possa ser chamada no início E após o cadastro.
  const carregarDrones = async () => {
    setIsLoading(true); // Mostra o "Carregando..." na tabela
    try {
      // --- QUANDO A API ESTIVER PRONTA ---
      // const response = await api.get('/drones')
      // setDrones(response.data)

      // --- POR ENQUANTO, USAMOS O MOCK ---
      // Simula um delay de rede
      await new Promise(res => setTimeout(res, 300));
      setDrones(MOCK_DRONES)
      
    } catch (err) {
      console.error("Falha ao carregar frota de drones", err)
    } finally {
      setIsLoading(false)
    }
  }

  // 4. CHAMAR A FUNÇÃO QUANDO A PÁGINA CARREGA
  useEffect(() => {
    carregarDrones()
  }, []) // O [] vazio garante que isso rode só uma vez

  // 5. FUNÇÃO PARA RECARREGAR APÓS CADASTRO
  const handleDroneCadastrado = () => {
    setIsModalOpen(false); // Fecha o modal
    carregarDrones(); // Recarrega a lista de drones
  }

  return (
    <div>
      {/* 6. RENDERIZAR O MODAL CONDICIONALMENTE */}
      {isModalOpen && (
        <DroneModal 
          onClose={() => setIsModalOpen(false)}
          onDroneCadastrado={handleDroneCadastrado}
        />
      )}

      <div className={styles.header}>
        <h1>Gerenciador de Frota de Drones</h1>
        {/* 7. LIGAR O BOTÃO */}
        <button 
          className={styles.registerButton}
          onClick={() => setIsModalOpen(true)} // Abre o modal
        >
          Cadastrar Novo Drone
        </button>
      </div>
      <p style={{ color: 'var(--color-text-secondary)', marginBottom: '24px' }}>
        Visualize e gerencie todos os drones de reconhecimento ativos.
      </p>

      {/* Tabela para listar os Drones (sem alteração) */}
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
              <tr key={drone.serial_number}>
                <td className={styles.serial}>{drone.serial_number}</td>
                <td>{drone.marca}</td>
                <td>{drone.fabricante}</td>
                <td>{drone.pais_origem}</td>
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  )
}

export default FrotaPage