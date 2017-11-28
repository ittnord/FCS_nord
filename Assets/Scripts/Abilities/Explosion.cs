namespace FCS.Abilities
{
    public class Explosion : FireBall
    {
        public override void OnInstantiate()
        {
            Explode(true);
            Destroy(gameObject);
        } 
    }
}