import React from 'react';
import { useNavigate } from 'react-router-dom'; // Para navegar para o dashboard
import styles from './LandingPage.module.css'; // Criaremos este CSS

// Ícone simples (pode substituir por um SVG mais elaborado ou imagem)
const LogoIcon = () => (
    <svg className={styles.logoIcon} width="80" height="80" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
        {/* Adaptei o ícone de Pato */}
        <path d="M15.5 14C17.433 14 19 12.433 19 10.5C19 8.567 17.433 7 15.5 7C13.567 7 12 8.567 12 10.5C12 12.433 13.567 14 15.5 14Z" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round"/>
         <path d="M7 20C7 17.2386 9.23858 15 12 15C12.5929 15 13.1652 15.101 13.7084 15.2892M9 12C9 14.2091 7.20914 16 5 16C2.79086 16 1 14.2091 1 12C1 9.79086 2.79086 8 5 8C7.20914 8 9 9.79086 9 12Z" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round"/>
        {/* Adiciona um círculo de "alvo" */}
         <circle cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="1"/>
         <line x1="12" y1="5" x2="12" y2="9" stroke="currentColor" strokeWidth="1"/>
         <line x1="12" y1="15" x2="12" y2="19" stroke="currentColor" strokeWidth="1"/>
         <line x1="5" y1="12" x2="9" y2="12" stroke="currentColor" strokeWidth="1"/>
         <line x1="15" y1="12" x2="19" y2="12" stroke="currentColor" strokeWidth="1"/>
    </svg>
);


const LandingPage = () => {
    const navigate = useNavigate();

    const handleAccess = () => {
        navigate('/dashboard'); 
    };

    return (
        <div className={styles.landingContainer}>
            <div className={styles.content}>
                <LogoIcon />
                <h1 className={styles.title}>DSIN</h1>
                <p className={styles.subtitle}>Operação Pato Primordial</p>
                <p className={styles.description}>
                    Acesso restrito ao painel de controlo. Identifique, analise e prepare-se para a captura.
                </p>
                <button className={styles.accessButton} onClick={handleAccess}>
                    Aceder ao Sistema
                </button>
            </div>
             <div className={styles.bgLines}>
                 <div className={styles.line}></div>
                 <div className={styles.line}></div>
                 <div className={styles.line}></div>
             </div>
        </div>
    );
};

export default LandingPage;