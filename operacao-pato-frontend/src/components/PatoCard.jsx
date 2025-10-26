import { Link } from "react-router-dom";
import styles from "./PatoCard.module.css";


const getStatusClass = (apiStatus) => {
   switch (apiStatus?.toLowerCase()) {
      case "desperto":
         return styles.statusDesperto;
      case "transe": 
      case "em transe": 
         return styles.statusEmTranse;
      case "hibernacaoprofunda": 
      case "hibernacao profunda": 
         return styles.statusHibernacao;
      default:
         return "";
   }
};

const PatoCard = ({ pato, imageUrl }) => {
   const statusClassName = getStatusClass(pato.status);

   return (
      <div className={styles.card}>
         <div className={styles.cardImageContainer}>
            {" "}
            <img
               src={imageUrl} 
               alt={`Pato ${pato.nome || pato.pontoReferencia || pato.id}`} 
               className={styles.patoImage} 
               onError={(e) => {
                  e.target.src = "/images/pato-default.png";
               }} 
            />
         </div>

         <div className={styles.cardContent}>
            <span className={`${styles.statusIndicator} ${statusClassName}`}>
               {pato.status}
            </span>

            <h3 className={styles.cardTitle}>
               {pato.nome || pato.pontoReferencia || `Pato ID: ${pato.id}`}
            </h3>
            <p className={styles.cardInfo}>
               Localização: {pato.cidade || "Desconhecida"},{" "}
               {pato.pais || "Desconhecido"}
            </p>
            <p className={styles.cardInfo}>
               Mutações: {pato.quantidadeMutacoes || 0}
            </p>

            <Link to={`/dashboard/pato/${pato.id}`} className={styles.detailsLink}>
               Analisar Sequência de DNA
            </Link>
         </div>
      </div>
   );
};

export default PatoCard;
