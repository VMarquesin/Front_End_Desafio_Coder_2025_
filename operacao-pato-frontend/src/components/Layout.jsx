import { NavLink } from 'react-router-dom'
// Usamos CSS Modules para que 'styles' afete SÓ este componente
import styles from './Layout.module.css' 

// Ícones simples em SVG como strings (você pode substituir por 'react-icons')
const IconMap = () => <svg viewBox="0 0 20 20" fill="currentColor" width="20" height="20"><path fillRule="evenodd" d="M12 1.5a.5.5 0 01.5.5v16a.5.5 0 01-.5.5h-10a.5.5 0 01-.5-.5v-16a.5.5 0 01.5-.5h10zM10 0a2 2 0 00-2 2v16a2 2 0 002 2h1a2 2 0 002-2V2a2 2 0 00-2-2h-1zM5 16.5a.5.5 0 01.5.5v1a.5.5 0 01-.5.5H3a.5.5 0 01-.5-.5v-1a.5.5 0 01.5-.5h2z" clipRule="evenodd"></path></svg>;
const IconBook = () => <svg viewBox="0 0 20 20" fill="currentColor" width="20" height="20"><path fillRule="evenodd" d="M3.5 2A1.5 1.5 0 002 3.5v13A1.5 1.5 0 003.5 18h13a1.5 1.5 0 001.5-1.5v-13A1.5 1.5 0 0016.5 2h-13zM12 11.5a.5.5 0 01.5-.5h2a.5.5 0 010 1h-2a.5.5 0 01-.5-.5zm-4-3a.5.5 0 01.5-.5h5a.5.5 0 010 1h-5a.5.5 0 01-.5-.5z" clipRule="evenodd"></path></svg>;

const Layout = ({ children }) => {
  return (
    <div className={styles.layoutContainer}>
      <nav className={styles.sidebar}>
        <div className={styles.logo}>
          DSIN
          <span>Operação Pato</span>
        </div>
        
        <ul className={styles.navList}>
          <li>
            <NavLink 
              to="/" 
              className={({ isActive }) => isActive ? styles.active : ''}
            >
              <IconMap />
              Dashboard (Mapa)
            </NavLink>
          </li>
          <li>
            <NavLink 
              to="/patopedia"
              className={({ isActive }) => isActive ? styles.active : ''}
            >
              <IconBook />
              Pato-pédia
            </NavLink>
          </li>
        </ul>
      </nav>
      
      <main className={styles.content}>
        {children}
      </main>
    </div>
  )
}

export default Layout