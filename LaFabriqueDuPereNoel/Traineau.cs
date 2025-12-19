// ============= CLASSE TRAINEAU =============
namespace LaFabriqueDuPereNoel
{
    public class Traineau
    {
        public Continent Continent { get; set; }
        public Stack<Lettre> Lettres { get; set; }
        public int CapaciteMax { get; set; }
        public bool EnVoyage { get; set; }
        public int HeuresVoyageRestantes { get; set; }

        public Traineau(Continent continent, int capacite)
        {
            Continent = continent;
            CapaciteMax = capacite;
            Lettres = new Stack<Lettre>();
            EnVoyage = false;
            HeuresVoyageRestantes = 0;
        }

        public bool PeutCharger()
        {
            return !EnVoyage && Lettres.Count < CapaciteMax;
        }

        public void ChargerLettre(Lettre lettre)
        {
            if (lettre == null) return;
            if (PeutCharger())
            {
                Lettres.Push(lettre);
            }
        }

        public bool EstPlein()
        {
            return Lettres.Count >= CapaciteMax;
        }

        public void PartirEnVoyage()
        {
            if (Lettres.Count > 0 && !EnVoyage)
            {
                EnVoyage = true;
                HeuresVoyageRestantes = 6;
            }
        }

        public List<Lettre> AvancerVoyage()
        {
            if (!EnVoyage) return null;

            HeuresVoyageRestantes--;

            if (HeuresVoyageRestantes <= 0)
            {
                List<Lettre> lettresLivrees = Lettres.ToList();
                Lettres.Clear();
                EnVoyage = false;
                return lettresLivrees;
            }

            return null;
        }
    }
}