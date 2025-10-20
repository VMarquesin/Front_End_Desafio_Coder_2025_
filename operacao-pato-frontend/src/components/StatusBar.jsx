import styles from './StatusBar.module.css'

const StatusBar = ({ label, current, max, color = 'var(--color-accent)' }) => {
  const percentage = max > 0 ? (current / max) * 100 : 0;

  return (
    <div className={styles.statusBarWrapper}>
      <div className={styles.labelContainer}>
        <span className={styles.label}>{label}</span>
        <span className={styles.value}>{Math.round(current)} / {max}</span>
      </div>
      <div className={styles.barOuter}>
        <div 
          className={styles.barInner} 
          style={{ 
            width: `${percentage}%`, 
            backgroundColor: color 
          }}
        />
      </div>
    </div>
  )
}

export default StatusBar