import { useState, useEffect } from "react"; 
import api from "../services/api";
import styles from "./CatalogModal.module.css";

const CatalogModal = ({ onClose, onPatoCatalogado }) => {
  
   const [droneSerial, setDroneSerial] = useState(""); 
   const [listaDeDrones, setListaDeDrones] = useState([]);
   const [isLoadingDrones, setIsLoadingDrones] = useState(true);

   const [alturaVal, setAlturaVal] = useState("");
   const [alturaUnidade, setAlturaUnidade] = useState("cm");
   const [pesoVal, setPesoVal] = useState("");

   const [pesoUnidade, setPesoUnidade] = useState("g");
   const [cidade, setCidade] = useState("");
   const [pais, setPais] = useState("");

   const [latitude, setLatitude] = useState("");
   const [longitude, setLongitude] = useState("");
   const [precisaoVal, setPrecisaoVal] = useState("");

   const [precisaoUnidade, setPrecisaoUnidade] = useState("m");
   const [pontoReferencia, setPontoReferencia] = useState("");
   const [statusHibernacao, setStatusHibernacao] = useState(
      "hibernacao profunda"
   );

   const [batimentosBpm, setBatimentosBpm] = useState("");
   const [qtdMutacoes, setQtdMutacoes] = useState("");
   const [superPoderNome, setSuperPoderNome] = useState("");

   const [superPoderDescricao, setSuperPoderDescricao] = useState("");
   const [superPoderClassificacao, setSuperPoderClassificacao] = useState("");
   const [isLoading, setIsLoading] = useState(false);
   const [error, setError] = useState(null);

  
   useEffect(() => {
      const carregarDrones = async () => {
         try {
            const response = await api.get("/drones");
            setListaDeDrones(response.data);
         } catch (err) {
            console.error("Falha ao carregar drones", err);
            setError("Não foi possível carregar a lista de drones.");
         } finally {
            setIsLoadingDrones(false);
         }
      };
      carregarDrones();
   }, []); 

   const handleSubmit = async (e) => {
      e.preventDefault();
      setIsLoading(true);
      setError(null);


      const dadosBrutos = {
         droneNumeroSerie: droneSerial, 
         alturaValor: parseFloat(alturaVal),
         alturaUnidade: alturaUnidade,
         pesoValor: parseFloat(pesoVal),
         pesoUnidade: pesoUnidade, 
         pais: pais || "",
         cidade: cidade || "",
         latitude: parseFloat(latitude),
         longitude: parseFloat(longitude),
         pontoReferencia: pontoReferencia || null,
         precisao: parseFloat(precisaoVal), 
         precisaoUnidade: precisaoUnidade, 
         status: statusHibernacao, 
         batimentosPorMinuto:
            statusHibernacao !== "Desperto" && batimentosBpm 
               ? parseInt(batimentosBpm)
               : null,
         quantidadeMutacoes: parseInt(qtdMutacoes), 
         poderNome: statusHibernacao === "Desperto" ? PoderNome || "" : "", 
         poderDescricao:
            statusHibernacao === "Desperto" ? PoderDescricao || "" : "",
         poderClassificacao:
            statusHibernacao === "Desperto"
               ? PoderClassificacao || ""
               : "",
         dataColetaUtc: new Date().toISOString(),
      };

 
      try {
         await api.post("/patos", dadosBrutos); 
         alert("Pato Primordial catalogado com sucesso!");
         onPatoCatalogado();
         onClose();
      } catch (err) {
         console.error("Erro ao catalogar Pato:", err);
         setError("Falha ao enviar dados. Verifique o console e a API.");
      } finally {
         setIsLoading(false);
      }
   };

   return (
      <div className={styles.modalOverlay}>
         <div className={styles.modalContent}>
            <form onSubmit={handleSubmit}>
               <div className={styles.modalHeader}>
                  <h2>Catalogar Novo Pato Primordial</h2>
                  <button
                     type="button"
                     className={styles.closeButton}
                     onClick={onClose}
                  >
                     ×
                  </button>
               </div>

               <div className={styles.modalBody}>
                  {/* --- Seção do Drone  --- */}
                  <fieldset>
                     <legend>Informações do Drone</legend>
                     <div className={styles.formGroup}>
                        <label>Nº de Série do Drone Responsável</label>
                        <select
                           value={droneSerial}
                           onChange={(e) => setDroneSerial(e.target.value)}
                           required
                           disabled={isLoadingDrones}
                        >
                           <option value="">
                              {isLoadingDrones
                                 ? "Carregando drones..."
                                 : "-- Selecione um Drone --"}
                           </option>

                           {listaDeDrones.map((drone) => (
                              <option
                                 key={drone.numeroSerie}
                                 value={drone.numeroSerie}
                              >
                                 {drone.numeroSerie}
                              </option>
                           ))}
                        </select>
                     </div>
                  </fieldset>

                  {/* Seção de Medidas Físicas */}
                  <fieldset>
                     <legend>Medidas Físicas</legend>
                     <div className={styles.formRow}>
                        <div className={styles.formGroup} style={{ flex: 3 }}>
                           <label>Altura</label>
                           <input
                              type="number"
                              value={alturaVal}
                              onChange={(e) => setAlturaVal(e.target.value)}
                              required
                           />
                        </div>
                        <div className={styles.formGroup} style={{ flex: 1 }}>
                           <label>Unidade</label>
                           <select
                              value={alturaUnidade}
                              onChange={(e) => setAlturaUnidade(e.target.value)}
                           >
                              <option value="cm">cm</option>
                              <option value="pés">pés (EUA)</option>
                           </select>
                        </div>
                     </div>
                     <div className={styles.formRow}>
                        <div className={styles.formGroup} style={{ flex: 3 }}>
                           <label>Peso</label>
                           <input
                              type="number"
                              value={pesoVal}
                              onChange={(e) => setPesoVal(e.target.value)}
                              required
                           />
                        </div>
                        <div className={styles.formGroup} style={{ flex: 1 }}>
                           <label>Unidade</label>
                           <select
                              value={pesoUnidade}
                              onChange={(e) => setPesoUnidade(e.target.value)}
                           >
                              <option value="g">g</option>
                              <option value="libras">libras (EUA)</option>
                           </select>
                        </div>
                     </div>
                  </fieldset>

                  <fieldset>
                     <legend>Localização (GPS)</legend>
                     <div className={styles.formRow}>
                        <div className={styles.formGroup}>
                           <label>Cidade</label>
                           <input
                              type="text"
                              value={cidade}
                              onChange={(e) => setCidade(e.target.value)}
                           />
                        </div>
                        <div className={styles.formGroup}>
                           <label>País</label>
                           <input
                              type="text"
                              value={pais}
                              onChange={(e) => setPais(e.target.value)}
                           />
                        </div>
                     </div>
                     <div className={styles.formRow}>
                        <div className={styles.formGroup}>
                           <label>Latitude</label>
                           <input
                              type="number"
                              step="any"
                              value={latitude}
                              onChange={(e) => setLatitude(e.target.value)}
                              required
                           />
                        </div>
                        <div className={styles.formGroup}>
                           <label>Longitude</label>
                           <input
                              type="number"
                              step="any"
                              value={longitude}
                              onChange={(e) => setLongitude(e.target.value)}
                              required
                           />
                        </div>
                     </div>
                     <div className={styles.formRow}>
                        <div className={styles.formGroup} style={{ flex: 3 }}>
                           <label>Precisão da Coleta</label>
                           <input
                              type="number"
                              step="any"
                              value={precisaoVal}
                              onChange={(e) => setPrecisaoVal(e.target.value)}
                              required
                           />
                        </div>
                        <div className={styles.formGroup} style={{ flex: 1 }}>
                           <label>Unidade</label>
                           <select
                              value={precisaoUnidade}
                              onChange={(e) =>
                                 setPrecisaoUnidade(e.target.value)
                              }
                           >
                              <option value="m">m</option>
                              <option value="cm">cm</option>
                              <option value="jardas">jardas (EUA)</option>
                           </select>
                        </div>
                     </div>
                     <div className={styles.formGroup}>
                        <label>Ponto de Referência (Opcional)</label>
                        <input
                           type="text"
                           value={pontoReferencia}
                           onChange={(e) => setPontoReferencia(e.target.value)}
                           placeholder="Ex: Pico da Neblina"
                        />
                     </div>
                  </fieldset>

                  <fieldset>
                     <legend>Status Biológico</legend>
                     <div className={styles.formRow}>
                        <div className={styles.formGroup}>
                           <label>Status de Hibernação</label>
                           <select
                              value={statusHibernacao}
                              onChange={(e) =>
                                 setStatusHibernacao(e.target.value)
                              }
                           >
                              <option value="hibernacao profunda">
                                 Hibernação Profunda
                              </option>
                              <option value="em transe">Em Transe</option>
                              <option value="desperto">Desperto</option>
                           </select>
                        </div>
                        {(statusHibernacao === "em transe" ||
                           statusHibernacao === "hibernacao profunda") && (
                           <div className={styles.formGroup}>
                              <label>Batimentos Cardíacos (bpm)</label>
                              <input
                                 type="number"
                                 value={batimentosBpm}
                                 onChange={(e) =>
                                    setBatimentosBpm(e.target.value)
                                 }
                                 placeholder="Apenas se dormente"
                              />
                           </div>
                        )}
                     </div>
                     {statusHibernacao === "desperto" && (
                        <div className={styles.superPoderGroup}>
                           <div className={styles.formGroup}>
                              <label>Super-poder (Nome)</label>
                              <input
                                 type="text"
                                 value={superPoderNome}
                                 onChange={(e) =>
                                    setSuperPoderNome(e.target.value)
                                 }
                                 placeholder="Ex: Tempestade Elétrica"
                              />
                           </div>
                           <div className={styles.formGroup}>
                              <label>Super-poder (Descrição)</label>
                              <textarea
                                 rows="3"
                                 value={superPoderDescricao}
                                 onChange={(e) =>
                                    setSuperPoderDescricao(e.target.value)
                                 }
                                 placeholder="Ex: Gera descargas elétricas em área."
                              ></textarea>
                           </div>
                           <div className={styles.formGroup}>
                              <label>Super-poder (Classificação)</label>
                              <input
                                 type="text"
                                 value={superPoderClassificacao}
                                 onChange={(e) =>
                                    setSuperPoderClassificacao(e.target.value)
                                 }
                                 placeholder="Ex: bélico, raro, alto risco"
                              />
                           </div>
                        </div>
                     )}
                     <div className={styles.formGroup}>
                        <label>Quantidade de Mutações</label>
                        <input
                           type="number"
                           value={qtdMutacoes}
                           onChange={(e) => setQtdMutacoes(e.target.value)}
                           required
                        />
                     </div>
                  </fieldset>

                  {error && <p className={styles.errorText}>{error}</p>}
               </div>

               <div className={styles.modalFooter}>
                  <button
                     type="button"
                     className={styles.buttonSecondary}
                     onClick={onClose}
                     disabled={isLoading}
                  >
                     Cancelar
                  </button>
                  <button
                     type="submit"
                     className={styles.buttonPrimary}
                     disabled={isLoading || isLoadingDrones}
                  >
                     {isLoading ? "Catalogando..." : "Salvar e Catalogar"}
                  </button>
               </div>
            </form>
         </div>
      </div>
   );
};

export default CatalogModal;
