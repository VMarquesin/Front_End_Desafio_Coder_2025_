import { useState, useEffect } from "react";
import PatoCard from "../components/PatoCard";
import api from "../services/api";
import styles from "./PatopediaPage.module.css";

const getPatoImage = (status) => {
   switch (
      status?.toLowerCase() 
   ) {
      case "desperto":
         return "/images/desperto.png";
      case "transe": 
      case "em transe":
         return "/images/transe.png";
      case "hibernacaoprofunda": 
      case "hibernacao profunda": 
         return "/images/hibernando.png";
      default:
         return "/images/padrao.png";
   }
};

const PatopediaPage = () => {
   const [patos, setPatos] = useState([]);
   const [isLoading, setIsLoading] = useState(true);
   const [error, setError] = useState(null);

   useEffect(() => {
      const carregarPatos = async () => {
         try {
            const response = await api.get("/patos"); 
            setPatos(response.data); 
         } catch (err) {
            setError("Falha ao carregar arquivos da Pato-pédia.");
            console.error(err);
         } finally {
            setIsLoading(false);
         }
      };
      carregarPatos();
   }, []);

   return (
      <div>
         <h1>Pato-pédia: Arquivos Genéticos</h1>
         <p
            style={{
               color: "var(--color-text-secondary)",
               marginBottom: "24px",
            }}
         >
            Galeria de todas as espécies catalogadas. Clique em um card para
            análise detalhada.
         </p>
         {isLoading && (
            <p className={styles.loadingText}>Carregando arquivos...</p>
         )}
         {error && <p className={styles.errorText}>{error}</p>}

         {/* Grade de Cards */}
         {!isLoading && !error && (
            <div className={styles.patoGrid}>
               {patos.map((pato) => (
                  <PatoCard
                     key={pato.id}
                     pato={pato}
                     imageUrl={getPatoImage(pato.status)} 
                  />
               ))}
            </div>
         )}
      </div>
   );
};

export default PatopediaPage;
