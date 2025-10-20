import { useState, useEffect } from 'react'
import PatoCard from '../components/PatoCard'
import api from '../services/api'
import styles from './PatopediaPage.module.css' 

// --- DADOS MOCKADOS ---

// Garanta que os campos batem com os que você usa no PatoCard.jsx
const MOCK_PATOS = [
  { 
    id: 1, 
    ponto_referencia: "Pico da Neblina", 
    cidade: "S. Isabel do Rio Negro", 
    pais: "Brasil",
    status_hibernacao: "desperto", 
    quantidade_mutacoes: 88 
  },
  { 
    id: 2, 
    ponto_referencia: "Estátua da Liberdade", 
    cidade: "Nova York", 
    pais: "EUA",
    status_hibernacao: "em transe", 
    quantidade_mutacoes: 15
  },
  { 
    id: 3, 
    ponto_referencia: "Lago Ness", 
    cidade: "Highlands", 
    pais: "Escócia",
    status_hibernacao: "hibernacao profunda", 
    quantidade_mutacoes: 42
  },
];
// ---------------------------------


const PatopediaPage = () => {
  const [patos, setPatos] = useState([])
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState(null)

  useEffect(() => {
    const carregarPatos = async () => {
      try {
        // --- QUANDO A API ESTIVER PRONTA ---
        // 1. Descomente a linha abaixo
        // const response = await api.get('/patos')
        
        // 2. Descomente a linha abaixo e apague a linha do MOCK
        // setPatos(response.data)

        // --- MOCK ---
        setPatos(MOCK_PATOS)

      } catch (err) {
        setError("Falha ao carregar arquivos da Pato-pédia.")
        console.error(err)
      } finally {
        setIsLoading(false)
      }
    }
    setTimeout(carregarPatos, 500)
  }, []) 

  return (
    <div>
      <h1>Pato-pédia: Arquivos Genéticos</h1>
      <p style={{ color: 'var(--color-text-secondary)', marginBottom: '24px' }}>
        Galeria de todas as espécies catalogadas. Clique em um card para análise detalhada.
      </p>

      {/* Exibe o status de carregamento/erro */}
      {isLoading && <p className={styles.loadingText}>Carregando arquivos...</p>}
      {error && <p className={styles.errorText}>{error}</p>}
      
      {/* A Grade de Cards */}
      {!isLoading && !error && (
        <div className={styles.patoGrid}>
          {patos.map(pato => (
            <PatoCard key={pato.id} pato={pato} />
          ))}
        </div>
      )}
    </div>
  )
}

export default PatopediaPage