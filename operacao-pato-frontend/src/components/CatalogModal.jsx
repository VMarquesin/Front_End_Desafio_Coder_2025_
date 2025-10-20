import { useState } from 'react'
import api from '../services/api'
import styles from './CatalogModal.module.css'

// 'onClose' (para fechar) e 'onPatoCatalogado' (para o mapa para atualizar)
const CatalogModal = ({ onClose, onPatoCatalogado }) => {
  // campos do formulário
  const [droneSerial, setDroneSerial] = useState('')
  const [droneMarca, setDroneMarca] = useState('')
  const [droneFabricante, setDroneFabricante] = useState('')
  const [dronePais, setDronePais] = useState('')

  const [alturaVal, setAlturaVal] = useState('')
  const [alturaUnidade, setAlturaUnidade] = useState('cm')
  
  const [pesoVal, setPesoVal] = useState('')
  const [pesoUnidade, setPesoUnidade] = useState('g')

  const [cidade, setCidade] = useState('')
  const [pais, setPais] = useState('')
  const [latitude, setLatitude] = useState('')
  const [longitude, setLongitude] = useState('')

  const [precisaoVal, setPrecisaoVal] = useState('')
  const [precisaoUnidade, setPrecisaoUnidade] = useState('m')

  const [pontoReferencia, setPontoReferencia] = useState('')
  const [statusHibernacao, setStatusHibernacao] = useState('hibernacao profunda')
  const [batimentosBpm, setBatimentosBpm] = useState('')
  const [qtdMutacoes, setQtdMutacoes] = useState('')

  const [superPoderNome, setSuperPoderNome] = useState('')
  const [superPoderDescricao, setSuperPoderDescricao] = useState('')
  const [superPoderClassificacao, setSuperPoderClassificacao] = useState('')

  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState(null)

  const handleSubmit = async (e) => {
    e.preventDefault()
    setIsLoading(true)
    setError(null)

    // 1. objeto JSON
    const dadosBrutos = {
      drone_info: {
        serial: droneSerial,
        marca: droneMarca,
        fabricante: droneFabricante,
        pais_origem: dronePais
      },
      altura: {
        valor: parseFloat(alturaVal),
        unidade: alturaUnidade
      },
      peso: {
        valor: parseFloat(pesoVal),
        unidade: pesoUnidade
      },
      localizacao: {
        cidade: cidade,
        pais: pais,
        latitude: parseFloat(latitude),
        longitude: parseFloat(longitude),
        precisao: {
          valor: parseFloat(precisaoVal),
          unidade: precisaoUnidade
        },
        ponto_referencia: pontoReferencia || null
      },
      status_hibernacao: statusHibernacao,
      batimentos_cardiacos_bpm: (statusHibernacao !== 'desperto' && batimentosBpm) ? parseInt(batimentosBpm) : null,
      quantidade_mutacoes: parseInt(qtdMutacoes),
      super_poder: (statusHibernacao === 'desperto' && superPoderNome) ? {
                    nome: superPoderNome,
                    descricao: superPoderDescricao,
                    classificacao: superPoderClassificacao
                    } : null   
    }

    // 2. API
    try {
      await api.post('/patos', dadosBrutos)
      alert('Pato Primordial catalogado com sucesso!')
      onPatoCatalogado() // recarregar no mapa
      onClose()
    } catch (err) {
      console.error("Erro ao catalogar Pato:", err)
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
            <h2>Catalogar Novo Pato Primordial</h2>
            <button type="button" className={styles.closeButton} onClick={onClose}>×</button>
          </div>
          
          <div className={styles.modalBody}>
            {/* Seção do Drone */}
            <fieldset>
              <legend>Informações do Drone</legend>
              <div className={styles.formRow}>
                <div className={styles.formGroup}>
                  <label>Nº de Série</label>
                  <input type="text" value={droneSerial} onChange={e => setDroneSerial(e.target.value)} required />
                </div>
                <div className={styles.formGroup}>
                  <label>Marca</label>
                  <input type="text" value={droneMarca} onChange={e => setDroneMarca(e.target.value)} required />
                </div>
              </div>
              <div className={styles.formRow}>
                <div className={styles.formGroup}>
                  <label>Fabricante</label>
                  <input type="text" value={droneFabricante} onChange={e => setDroneFabricante(e.target.value)} required />
                </div>
                <div className={styles.formGroup}>
                  <label>País de Origem</label>
                  <input type="text" value={dronePais} onChange={e => setDronePais(e.target.value)} required />
                </div>
              </div>
            </fieldset>

            {/* Seção de Medidas Físicas */}
            <fieldset>
              <legend>Medidas Físicas</legend>
              <div className={styles.formRow}>
                <div className={styles.formGroup} style={{ flex: 3 }}>
                  <label>Altura</label>
                  <input type="number" value={alturaVal} onChange={e => setAlturaVal(e.target.value)} required />
                </div>
                <div className={styles.formGroup} style={{ flex: 1 }}>
                  <label>Unidade</label>
                  <select value={alturaUnidade} onChange={e => setAlturaUnidade(e.target.value)}>
                    <option value="cm">cm</option>
                    <option value="pés">pés (EUA)</option>
                  </select>
                </div>
              </div>
              <div className={styles.formRow}>
                <div className={styles.formGroup} style={{ flex: 3 }}>
                  <label>Peso</label>
                  <input type="number" value={pesoVal} onChange={e => setPesoVal(e.target.value)} required />
                </div>
                <div className={styles.formGroup} style={{ flex: 1 }}>
                  <label>Unidade</label>
                  <select value={pesoUnidade} onChange={e => setPesoUnidade(e.target.value)}>
                    <option value="g">g</option>
                    <option value="libras">libras (EUA)</option>
                  </select>
                </div>
              </div>
            </fieldset>
            
            {/* Seção de Localização */}
            <fieldset>
              <legend>Localização (GPS)</legend>
              <div className={styles.formRow}>
                <div className={styles.formGroup}>
                  <label>Cidade</label>
                  <input type="text" value={cidade} onChange={e => setCidade(e.target.value)} />
                </div>
                <div className={styles.formGroup}>
                  <label>País</label>
                  <input type="text" value={pais} onChange={e => setPais(e.target.value)} />
                </div>
              </div>
              <div className={styles.formRow}>
                <div className={styles.formGroup}>
                  <label>Latitude</label>
                  <input type="number" step="any" value={latitude} onChange={e => setLatitude(e.target.value)} required />
                </div>
                <div className={styles.formGroup}>
                  <label>Longitude</label>
                  <input type="number" step="any" value={longitude} onChange={e => setLongitude(e.target.value)} required />
                </div>
              </div>
              <div className={styles.formRow}>
                <div className={styles.formGroup} style={{ flex: 3 }}>
                  <label>Precisão da Coleta</label>
                  <input type="number" step="any" value={precisaoVal} onChange={e => setPrecisaoVal(e.target.value)} required />
                </div>
                <div className={styles.formGroup} style={{ flex: 1 }}>
                  <label>Unidade</label>
                  <select value={precisaoUnidade} onChange={e => setPrecisaoUnidade(e.target.value)}>
                    <option value="m">m</option>
                    <option value="cm">cm</option>
                    <option value="jardas">jardas (EUA)</option>
                  </select>
                </div>
              </div>
              <div className={styles.formGroup}>
                <label>Ponto de Referência (Opcional)</label>
                <input type="text" value={pontoReferencia} onChange={e => setPontoReferencia(e.target.value)} placeholder="Ex: Pico da Neblina" />
              </div>
            </fieldset>

            {/* Seção de Status */}
            <fieldset>
              <legend>Status Biológico</legend>
              <div className={styles.formRow}>
                <div className={styles.formGroup}>
                  <label>Status de Hibernação</label>
                  <select value={statusHibernacao} onChange={e => setStatusHibernacao(e.target.value)}>
                    <option value="hibernacao profunda">Hibernação Profunda</option>
                    <option value="em transe">Em Transe</option>
                    <option value="desperto">Desperto</option>
                  </select>
                </div>
                {/* CONDIÇÃO BATIMENTOS */}
                {(statusHibernacao === 'em transe' || statusHibernacao === 'hibernacao profunda') && (
                  <div className={styles.formGroup}>
                    <label>Batimentos Cardíacos (bpm)</label>
                    <input type="number" value={batimentosBpm} onChange={e => setBatimentosBpm(e.target.value)} placeholder="Apenas se dormente" />
                  </div>
                )}
              </div>

                {/* CONDIÇÃO CLASSIFICAÇÃO */}
              {statusHibernacao === 'desperto' && (
                <div className={styles.superPoderGroup}>
                  <div className={styles.formGroup}>
                    <label>Super-poder (Nome)</label>
                    <input type="text" value={superPoderNome} onChange={e => setSuperPoderNome(e.target.value)} placeholder="Ex: Tempestade Elétrica" />
                  </div>
                  <div className={styles.formGroup}>
                    <label>Super-poder (Descrição)</label>
                    {/* Textarea é melhor para descrições longas */}
                    <textarea rows="3" value={superPoderDescricao} onChange={e => setSuperPoderDescricao(e.target.value)} placeholder="Ex: Gera descargas elétricas em área."></textarea>
                  </div>
                  <div className={styles.formGroup}>
                    <label>Super-poder (Classificação)</label>
                    <input type="text" value={superPoderClassificacao} onChange={e => setSuperPoderClassificacao(e.target.value)} placeholder="Ex: bélico, raro, alto risco" />
                  </div>
                </div>
              )}

              <div className={styles.formGroup}>
                <label>Quantidade de Mutações</label>
                <input type="number" value={qtdMutacoes} onChange={e => setQtdMutacoes(e.target.value)} required />
              </div>
            </fieldset>

            {error && <p className={styles.errorText}>{error}</p>}
          </div>

          <div className={styles.modalFooter}>
            <button type="button" className={styles.buttonSecondary} onClick={onClose} disabled={isLoading}>
              Cancelar
            </button>
            <button type="submit" className={styles.buttonPrimary} disabled={isLoading}>
              {isLoading ? 'Catalogando...' : 'Salvar e Catalogar'}
            </button>
          </div>
        </form>
      </div>
    </div>
  )
}

export default CatalogModal