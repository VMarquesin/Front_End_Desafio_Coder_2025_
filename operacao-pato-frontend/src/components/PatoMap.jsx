import { MapContainer, TileLayer, Marker, Popup } from 'react-leaflet'
import { useState, useEffect } from 'react'
import api from '../services/api' 

// Posição inicial do mapa
const center = [-15.793889, -47.882778] 

// --- DADOS MOCKADOS ---
const MOCK_PATOS = [
  { id: 1, nome: "BR-001 (Pico da Neblina)", lat: -0.413, lon: -65.019, risco: "Extremo (Desperto)" },
  { id: 2, nome: "USA-007 (Estátua da Liberdade)", lat: 40.7128, lon: -74.0060, risco: "Médio (Em Transe)" },
];

// const PatoMap = () => {

//   const [patos, setPatos] = useState([])


//   useEffect(() => {
//     const carregarPatos = async () => {
//       try {
//         const response = await api.get('/patos') 
//         setPatos(response.data)
//       } catch (error) {
//         console.error("Erro ao carregar patos da API!", error)
//       }
//     }
    
//     carregarPatos()
//   }, [])
// ---------------------------------

const PatoMap = () => {
  return (
    <MapContainer 
      center={center} 
      zoom={4} 
      scrollWheelZoom={true} 
      style={{ height: '100%', width: '100%' }}
    >
      <TileLayer
        attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
      />
      
      {MOCK_PATOS.map(pato => (
        <Marker key={pato.id} position={[pato.lat, pato.lon]}>
          <Popup>
            <b>{pato.nome}</b><br />
            Risco: {pato.risco} <br />
            <a href={`/pato/${pato.id}`}>Ver Detalhes...</a>
          </Popup>
        </Marker>
      ))}
    </MapContainer>
  )
}

export default PatoMap