import { Routes, Route } from 'react-router-dom';
import Layout from './components/Layout'; 
import LandingPage from './pages/LandingPage'; 
import DashboardPage from './pages/DashboardPage';
import PatopediaPage from './pages/PatopediaPage';
import PatoDetailPage from './pages/PatoDetailPage';
import MissionControlPage from './pages/MissionControlPage';
import FrotaPage from './pages/FrotaPage';

function App() {
  return (
    <Routes>
      {/* Página inicial (sem layout) */}
      <Route path="/" element={<LandingPage />} /> 

      {/* Rota pública fora do dashboard */}
      <Route path="/pato/:id" element={<PatoDetailPage />} />

      {/* Grupo de rotas internas que usam o Layout */}
      <Route 
        path="/dashboard/*" 
        element={
          <Layout> 
            <Routes> 
              <Route index element={<DashboardPage />} /> 
              <Route path="patopedia" element={<PatopediaPage />} />
              <Route path="pato/:id" element={<PatoDetailPage />} />
              <Route path="missao/:patoId" element={<MissionControlPage />} />
              <Route path="frota" element={<FrotaPage />} />
            </Routes>
          </Layout>
        } 
      />
    </Routes>
  );
}

export default App;
