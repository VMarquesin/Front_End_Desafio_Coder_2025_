import { useEffect, useRef } from 'react'
import styles from './CombatLog.module.css'

const CombatLog = ({ logs }) => {
  const logEndRef = useRef(null);
  useEffect(() => {
    logEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [logs]);

  return (
    <div className={styles.logWrapper}>
      <div className={styles.logHeader}>LOG DE COMBATE</div>
      <div className={styles.logContent}>
        {logs.map((log, index) => (
          <p key={index} className={styles.logEntry}>
            <span className={styles.logTimestamp}>[T-{index + 1}]</span> {log}
          </p>
        ))}
        <div ref={logEndRef} />
      </div>
    </div>
  )
}

export default CombatLog