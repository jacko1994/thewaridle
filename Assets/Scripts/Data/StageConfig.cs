[System.Serializable]
public class StageConfig
{
    public int numberOfEnemies;
    public int enemyHP;
    public int enemyATK;

    public StageConfig(int numberOfEnemies, int enemyHP, int enemyATK)
    {
        this.numberOfEnemies = numberOfEnemies;
        this.enemyHP = enemyHP;
        this.enemyATK = enemyATK;
    }
}
