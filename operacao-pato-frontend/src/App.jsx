import { Routes, Route } from 'react-router-dom'
import Layout from './components/Layout'
import DashboardPage from './pages/DashboardPage'
import PatopediaPage from './pages/PatopediaPage'
import PatoDetailPage from './pages/PatoDetailPage'
import MissionControlPage from './pages/MissionControlPage'

function App() {
  return (
    <Layout>
      <Routes>
        {/* Rota principal (Módulo 1) */}
        <Route path="/" element={<DashboardPage />} />
        
        {/* Rota da Pato-pédia (Módulo 2) */}
        <Route path="/patopedia" element={<PatopediaPage />} />
        
        {/* Rota de Detalhes de 1 Pato (usando ID) */}
        <Route path="/pato/:id" element={<PatoDetailPage />} />
        
        {/* Rota do Combate (Módulo 3) */}
        <Route path="/missao/:patoId" element={<MissionControlPage />} />
      </Routes>
    </Layout>
  )
}

export default App