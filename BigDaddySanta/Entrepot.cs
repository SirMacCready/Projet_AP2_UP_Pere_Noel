
// ============= CLASSE ENTREPOT =============

namespace BigDaddySanta
{
    public class Entrepot
    {
        public Continent Continent { get; set; }
        public Stack<Lettre> Lettres { get; set; }

        public Entrepot(Continent continent)
        {
            Continent = continent;
            Lettres = new Stack<Lettre>();
        }

        public void AjouterLettres(List<Lettre> lettres)
        {
            foreach (Lettre lettre in lettres)
            {
                Lettres.Push(lettre);
            }
        }

        public int[] GetStatistiques()
        {
            int[] stats = new int[5];

            foreach (Lettre lettre in Lettres)
            {
                stats[(int)lettre.Cadeau]++;
            }

            return stats;
        }
    }
}