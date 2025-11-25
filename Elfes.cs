    // ============= CLASSE ELFE =============

    public class Elfe
    {
        public int Id { get; set; }
        public Continent Continent { get; set; }
        public Traineau Traineau { get; set; }

        public Elfe(int id, Continent continent, int capaciteTraineau)
        {
            Id = id;
            Continent = continent;
            Traineau = new Traineau(continent, capaciteTraineau);
        }

        public bool PeutRecevoirLettre()
        {
            return Traineau.PeutCharger();
        }

        public void RecevoirLettre(Lettre lettre)
        {
            Traineau.ChargerLettre(lettre);

            if (Traineau.EstPlein())
            {
                Traineau.PartirEnVoyage();
            }
        }
    }
