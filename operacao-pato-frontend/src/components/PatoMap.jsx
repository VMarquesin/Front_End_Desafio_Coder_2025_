import { MapContainer, TileLayer, Marker, Popup } from "react-leaflet";
import { useState, useEffect } from "react";
import api from "../services/api";

import { Link } from "react-router-dom";

// Posição inicial do mapa
const center = [-15.793889, -47.882778];

const formatStatus = (status) => {
   if (!status) return "Desconhecido";
   return status
      .replace(/([A-Z])/g, " $1")
      .replace(/^./, (str) => str.toUpperCase());
};

const PatoMap = () => {
   const [patos, setPatos] = useState([]);
   const [isLoading, setIsLoading] = useState(true);
   const [error, setError] = useState(null);

   useEffect(() => {
      const carregarPatos = async () => {
         setIsLoading(true);
         setError(null);
         try {
            const response = await api.get("/patos");
            setPatos(response.data);
         } catch (err) {
            console.error("Erro ao carregar patos da API!", err);
            setError("Não foi possível carregar os dados dos patos.");
         } finally {
            setIsLoading(false);
         }
      };

      carregarPatos();
   }, []);

   if (isLoading) {
      return <div>Carregando mapa e dados dos patos...</div>;
   }
   if (error) {
      return <div style={{ color: "red" }}>Erro: {error}</div>;
   }

   return (
      <MapContainer
         center={center}
         zoom={4}
         scrollWheelZoom={true}
         style={{ height: "100%", width: "100%" }}
      >
         <TileLayer
            attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
            url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
         />
         // ...
         {patos.map((pato) => (
            <Marker key={pato.id} position={[pato.latitude, pato.longitude]}>
               <Popup>
                  <b>
                     {pato.nome ||
                        pato.pontoReferencia ||
                        `Pato: ${pato.id.substring(0, 8)}...`}
                  </b>
                  <br />
                  Status: {formatStatus(pato.status)} <br />
                  <Link to={`/dashboard/pato/${pato.id}`}>Ver Detalhes...</Link>
               </Popup>
            </Marker>
         ))}
         // ...
      </MapContainer>
   );
};

export default PatoMap;
