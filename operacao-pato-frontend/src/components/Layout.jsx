import { NavLink } from 'react-router-dom'
import styles from './Layout.module.css' 

// Ícones
const IconMap = () => <svg viewBox="0 0 20 20" fill="currentColor" width="20" height="20"><path fillRule="evenodd" d="M12 1.5a.5.5 0 01.5.5v16a.5.5 0 01-.5.5h-10a.5.5 0 01-.5-.5v-16a.5.5 0 01.5-.5h10zM10 0a2 2 0 00-2 2v16a2 2 0 002 2h1a2 2 0 002-2V2a2 2 0 00-2-2h-1zM5 16.5a.5.5 0 01.5.5v1a.5.5 0 01-.5.5H3a.5.5 0 01-.5-.5v-1a.5.5 0 01.5-.5h2z" clipRule="evenodd"></path></svg>;
const IconBook = () => <svg viewBox="0 0 20 20" fill="currentColor" width="20" height="20"><path fillRule="evenodd" d="M3.5 2A1.5 1.5 0 002 3.5v13A1.5 1.5 0 003.5 18h13a1.5 1.5 0 001.5-1.5v-13A1.5 1.5 0 0016.5 2h-13zM12 11.5a.5.5 0 01.5-.5h2a.5.5 0 010 1h-2a.5.5 0 01-.5-.5zm-4-3a.5.5 0 01.5-.5h5a.5.5 0 010 1h-5a.5.5 0 01-.5-.5z" clipRule="evenodd"></path></svg>;
const IconDrone = () => <svg viewBox="0 0 20 20" fill="currentColor" width="20" height="20"><path d="M10 2.5a.5.5 0 01.5.5v5a.5.5 0 01-1 0v-5a.5.5 0 01.5-.5zM8 8a.5.5 0 000 1h4a.5.5 0 000-1H8zM7.5 5a.5.5 0 00-1 0v1a.5.5 0 001 0V5zM12.5 5a.5.5 0 00-1 0v1a.5.5 0 001 0V5z"></path><path fillRule="evenodd" d="M2.77 11.516l1.06-.425a1 1 0 011.132.316l1.3 1.624A.5.5 0 006.8 13h6.4a.5.5 0 00.536-.717l-1.3-1.625a1 1 0 01.1-1.442l1.06-.425a1 1 0 011.332.93v1.897C16 13.91 13.313 16 10 16s-6-2.09-6-4.667v-1.897a1 1 0 011.332-.931L4.43 12a1 1 0 01.1 1.442L5.83 15.07a.5.5 0 00.536.717h.834a.5.5 0 00.5-.5v-1.5a.5.5 0 01.5-.5h2.6a.5.5 0 01.5.5v1.5a.5.5 0 00.5.5h.834a.5.5 0 00.536-.717l1.3-1.625a1 1 0 01.1-1.442l-1.06-.425a1 1 0 01-.564-1.298l.53-1.236A.5.5 0 0013.5 8.5h-7a.5.5 0 00-.468.324l.53 1.236a1 1 0 01-.563 1.298z" clipRule="evenodd"></path></svg>;


const Layout = ({ children }) => {
  return (
    <div className={styles.layoutContainer}>
      <nav className={styles.sidebar}>
        {/* ... (logo) ... */}
        <ul className={styles.navList}>
          <li>
            <NavLink 
              to="/dashboard" // MUDANÇA 1: Aponta para /dashboard
              end // Adicione "end" para que não fique ativo em outras rotas
              className={({ isActive }) => isActive ? styles.active : ''}
            >
              <IconMap />
              Dashboard (Mapa)
            </NavLink>
          </li>
          <li>
            <NavLink 
              to="/dashboard/patopedia" // MUDANÇA 2: Caminho completo
              className={({ isActive }) => isActive ? styles.active : ''}
            >
              <IconBook />
              Pato-pédia
            </NavLink>
          </li>
          <li>
            <NavLink 
              to="/dashboard/frota" // MUDANÇA 3: Caminho completo
              className={({ isActive }) => isActive ? styles.active : ''}
            >
              <IconDrone />
              Frota de Drones
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

export default Layout;