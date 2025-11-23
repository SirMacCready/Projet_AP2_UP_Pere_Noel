    // ============= CLASSE NAIN =============

    public class Nain
    {
        public int Id { get; set; }
        public StatutEmploye Statut { get; set; }
        public Lettre LettreEnCours { get; set; }
        public int HeuresRestantes { get; set; }

        public Nain(int id)
        {
            Id = id;
            Statut = StatutEmploye.EnAttente;
            LettreEnCours = null;
            HeuresRestantes = 0;
        }

        public void CommencerEmballage(Lettre lettre)
        {
            if (lettre == null) return;
            LettreEnCours = lettre;
            HeuresRestantes = 2; 
            Statut = StatutEmploye.EnTravail;
        }

        public Lettre Travailler()
        {
            if (Statut != StatutEmploye.EnTravail || LettreEnCours == null)
                return null;

            HeuresRestantes--;

            if (HeuresRestantes <= 0)
            {
                Lettre lettreTerminee = LettreEnCours;
                LettreEnCours = null;
                Statut = StatutEmploye.EnAttente;
                return lettreTerminee;
            }

            return null;
        }
    }