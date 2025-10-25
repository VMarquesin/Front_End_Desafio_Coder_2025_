import { useState } from 'react'
import api from '../services/api'
// Importaremos seu próprio CSS (que será uma cópia do outro modal)
import styles from './DroneModal.module.css'

const DroneModal = ({ onClose, onDroneCadastrado }) => {
  // States para os 4 campos
  const [serial, setSerial] = useState('')
  const [marca, setMarca] = useState('')
  const [fabricante, setFabricante] = useState('')
  const [paisOrigem, setPaisOrigem] = useState('')

  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState(null)

  const handleSubmit = async (e) => {
    e.preventDefault()
    setIsLoading(true)
    setError(null)

    // 1. Montar o objeto JSON para a API /drones
    const droneData = {
      numeroSerie: serial,
      marca: marca,
      fabricante: fabricante,
      paisOrigem: paisOrigem
    }

    // 2. Enviar para a API
    try {
      // Fale com seu time de backend sobre este endpoint!
      await api.post('/drones', droneData) 
      alert('Drone cadastrado com sucesso!')
      onDroneCadastrado() // Avisa a FrotaPage para recarregar
      onClose() // Fecha o modal
    } catch (err) {
      console.error("Erro ao cadastrar Drone:", err)
      setError("Falha ao enviar dados. Verifique o console e a API.")
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <div className={styles.modalOverlay}>
      <div className={styles.modalContent}>
        <form onSubmit={handleSubmit}>
          <div className={styles.modalHeader}>
            <h2>Cadastrar Novo Drone na Frota</h2>
            <button type="button" className={styles.closeButton} onClick={onClose}>×</button>
          </div>
          
          <div className={styles.modalBody}>
            <fieldset>
              <legend>Informações do Equipamento</legend>
              <div className={styles.formGroup}>
                <label>Nº de Série (ID Único)</label>
                <input type="text" value={serial} onChange={e => setSerial(e.target.value)} required />
              </div>
              <div className={styles.formGroup}>
                <label>Marca</label>
                <input type="text" value={marca} onChange={e => setMarca(e.target.value)} required />
              </div>
              <div className={styles.formGroup}>
                <label>Fabricante</label>
                <input type="text" value={fabricante} onChange={e => setFabricante(e.target.value)} required />
              </div>
              <div className={styles.formGroup}>
                <label>País de Origem</label>
                <input type="text" value={paisOrigem} onChange={e => setPaisOrigem(e.target.value)} required />
              </div>
            </fieldset>

            {error && <p className={styles.errorText}>{error}</p>}
          </div>

          <div className={styles.modalFooter}>
            <button type="button" className={styles.buttonSecondary} onClick={onClose} disabled={isLoading}>
              Cancelar
            </button>
            <button type="submit" className={styles.buttonPrimary} disabled={isLoading}>
              {isLoading ? 'Salvando...' : 'Salvar Drone'}
            </button>
          </div>
        </form>
      </div>
    </div>
  )
}

export default DroneModal