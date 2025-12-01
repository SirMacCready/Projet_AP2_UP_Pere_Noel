namespace BigDaddySanta
{
    // ============= ENUMS ET STRUCTURES =============

    public enum Continent
    {
        Europe,
        Asie ,
        Afrique,
        Amerique,
        Oceanie
    }

    public enum StatutEmploye
    {
        EnTravail,
        EnAttente,
        EnRepos
    }

    public enum Cadeau
    {
        Nounours,
        Tricycle,
        Jumelles,
        GeekyJunior,
        Ordinateur,
    }
    // ============= CLASSE SIMULATION =============

    public class Simulation // La classe qui gère la simulation.
    {
        // Paramètres configurables
        public int NbLutins { get; set; }
        public int NbNains { get; set; }
        public int NbEnfants { get; set; }
        public int NbLettresParHeure { get; set; }
        public int NbJouetParTraineau { get; set; }

        // Personnel
        public List<Lutin> Lutins { get; set; }
        public List<Nain> Nains { get; set; }
        public List<Elfe> Elfes { get; set; }

        // Files et piles
        public Stack<Lettre> BureauPereNoel { get; set; }
        public Queue<Lettre> FileAttenteFabrication { get; set; }
        public Queue<Lettre> FileAttenteEmballage { get; set; }
        public Queue<Lettre>[] FilesAttenteContinents { get; set; }
        // Entrepôts (un par continent, indexé par l'enum Continent)
        public Entrepot[] Entrepots { get; set; }

        // Temps et coûts
        public int HeureActuelle { get; set; }
        public int JourActuel { get; set; }
        public double CoutTotal { get; set; }
        public List<double> CoutsParHeure { get; set; }

        // Gestion du personnel
        public int DerniereModificationLutins { get; set; }
        public int DerniereModificationNains { get; set; }

        // Générateur aléatoire
        private Random random;

        // La mise en route de la simulation avec la mise à jour de tous les paramètres.
        public Simulation(int nbLutins, int nbNains, int nbEnfants, int nbLettresParHeure, int nbJouetParTraineau)
        {
            NbLutins = nbLutins;
            NbNains = nbNains;
            NbEnfants = nbEnfants;
            NbLettresParHeure = nbLettresParHeure;
            NbJouetParTraineau = nbJouetParTraineau;

            random = new Random();
            HeureActuelle = 0;
            JourActuel = 1;
            CoutTotal = 0;
            CoutsParHeure = new List<double>();
            DerniereModificationLutins = -12;   // Pour bien gérer le fait qu'on ne peut recruter des lutins pendant 12 heures si on en a mis en repos.
            DerniereModificationNains = -24;    // Pour bien gérer le fait qu'on ne peut recruter des nains pendant 12 heures si on en a mis en repos.

            // Initialiser les lutins
            Lutins = new List<Lutin>();
            for (int i = 0; i < nbLutins; i++)
            {
                Lutins.Add(new Lutin(i + 1));
            }

            // Initialiser les nains
            Nains = new List<Nain>();
            for (int i = 0; i < nbNains; i++)
            {
                Nains.Add(new Nain(i + 1));
            }

            // Initialiser les elfes (5, un par continent)
            Elfes = new List<Elfe>();
            int elfId = 1;
            foreach (Continent continent in Enum.GetValues(typeof(Continent)))
            {
                Elfes.Add(new Elfe(elfId++, continent, nbJouetParTraineau));
            }

            // Initialiser les structures de données
            BureauPereNoel = new Stack<Lettre>();
            FileAttenteFabrication = new Queue<Lettre>();
            FileAttenteEmballage = new Queue<Lettre>();

            FilesAttenteContinents = new Queue<Lettre>[5];
            for (int i = 0; i < 5; i++)
            {
                FilesAttenteContinents[i] = new Queue<Lettre>();
            }

            Entrepots = new Entrepot[5];
            for (int i = 0; i < 5; i++)
            {
                Entrepots[i] = new Entrepot((Continent)i);
            }

            // Générer toutes les lettres
            GenererLettres();
        }

        private void GenererLettres()
{

    List<Lettre> toutesLesLettres = new List<Lettre>();

    for (int i = 0; i < NbEnfants; i++)
    {
        string nom = DonneesSimulation.prenoms[random.Next(DonneesSimulation.prenoms.Length)];
        string adresse = DonneesSimulation.adresses[random.Next(DonneesSimulation.adresses.Length)];
        int age = random.Next(0, 19);

        // Ici, on ajoute un identifiant unique (i+1) à la lettre
        toutesLesLettres.Add(new Lettre(nom, adresse, age, i + 1));
    }

    foreach (Lettre lettre in toutesLesLettres)
    {
        BureauPereNoel.Push(lettre);
    }
}
        public void AvancerUneHeure()
        {
            // JE FAIS UN EFFORT POUR COMMENTER CHAQUE ÉTAPE CLAIREMENT CLOE !
            // Arrivée de nouvelles lettres sur le bureau du Père Noël (au début de l'heure)
            int maxArrivees = Math.Min(NbLettresParHeure, BureauPereNoel.Count);
            int nbArrivees = random.Next(maxArrivees + 1); // entre 0 et maxArrivees
            for (int i = 0; i < nbArrivees; i++)
            {
                FileAttenteFabrication.Enqueue(BureauPereNoel.Pop());
            }

            // Assigner des lettres aux lutins libres
            foreach (Lutin lutin in Lutins)
            {
                if (lutin.Statut == StatutEmploye.EnAttente && FileAttenteFabrication.Count > 0)
                {
                    Lettre lettre = FileAttenteFabrication.Dequeue();
                    lutin.CommencerFabrication(lettre);
                }
            }

            // Les lutins travaillent -> certaines lettres sont terminées
            List<Lettre> fabriques = new List<Lettre>();
            foreach (Lutin lutin in Lutins)
            {
                Lettre termine = lutin.Travailler();
                if (termine != null)
                {
                    fabriques.Add(termine);
                }
            }

            // Les jouets fabriqués vont soit aux nains (si dispo) soit en file d'attente emballage
            foreach (Lettre l in fabriques)
            {
                FileAttenteEmballage.Enqueue(l);
            }

            // Assigner des emballages aux nains libres
            foreach (Nain nain in Nains)
            {
                if (nain.Statut == StatutEmploye.EnAttente && FileAttenteEmballage.Count > 0)
                {
                    Lettre lettre = FileAttenteEmballage.Dequeue();
                    nain.CommencerEmballage(lettre);
                }
            }

            // Les nains travaillent -> certaines lettres sont emballées
            List<Lettre> emballes = new List<Lettre>();
            foreach (Nain nain in Nains)
            {
                Lettre termine = nain.Travailler();
                if (termine != null)
                {
                    emballes.Add(termine);
                }
            }

            // Les lettres emballées sont placées dans la file du continent correspondant
            foreach (Lettre l in emballes)
            {
                FilesAttenteContinents[(int)l.Continent].Enqueue(l);
            }

            // Les elfes reçoivent les lettres depuis les files continentales si possible
            foreach (Elfe elfe in Elfes)
            {
                Queue<Lettre> file = FilesAttenteContinents[(int)elfe.Continent];
                while (file.Count > 0 && elfe.Traineau.PeutCharger())
                {
                    Lettre lettre = file.Dequeue();
                    elfe.RecevoirLettre(lettre);
                }
            }

            // Avancer les voyages des traîneaux et récupérer les livraisons
            for (int i = 0; i < Elfes.Count; i++)
            {
                Elfe elfe = Elfes[i];
                List<Lettre> livrees = elfe.Traineau.AvancerVoyage();
                if (livrees != null && livrees.Count > 0)
                {
                    Entrepots[(int)elfe.Continent].AjouterLettres(livrees);
                }
            }

            // Calcul des coûts pour cette heure
            double coutHeure = 0;

            // Lutins: 1.5 pièce/h s'ils travaillent, 1 pièce/h s'ils sont en attente, 0 s'ils sont en repos
            foreach (Lutin lutin in Lutins)
            {
                if (lutin.Statut == StatutEmploye.EnTravail) coutHeure += 1.5;
                else if (lutin.Statut == StatutEmploye.EnAttente) coutHeure += 1.0;
            }

            // Nains: 1 pièce/h s'ils travaillent, 0.5 s'ils sont en attente
            foreach (Nain nain in Nains)
            {
                if (nain.Statut == StatutEmploye.EnTravail) coutHeure += 1.0;
                else if (nain.Statut == StatutEmploye.EnAttente) coutHeure += 0.5;
            }

            // Elfes: 2 pièces/h en voyage, 1.5 pièces/h si en chargement/présence de colis
            foreach (Elfe elfe in Elfes)
            {
                if (elfe.Traineau.EnVoyage) coutHeure += 2.0;
                else
                {
                    // Si l'elfe a chargé quelque chose (lettres sur le traîneau) ou s'il reste des lettres à charger -> coût de chargement
                    if (elfe.Traineau.Lettres.Count > 0 || FilesAttenteContinents[(int)elfe.Continent].Count > 0)
                        coutHeure += 1.5;
                }
            }

            CoutTotal += coutHeure;
            CoutsParHeure.Add(coutHeure);

            // Avancer le temps
            HeureActuelle++;
            if (HeureActuelle % 12 == 0)
            {
                // fin de journée de travail, on passe au jour suivant
                JourActuel++;
            }
        }

        public bool ToutesLettresTraitees()
        {
            if (BureauPereNoel.Count > 0) return false;
            if (FileAttenteFabrication.Count > 0) return false;
            if (FileAttenteEmballage.Count > 0) return false;

            // Vérifier si des lutins travaillent
            foreach (Lutin lutin in Lutins)
            {
                if (lutin.Statut == StatutEmploye.EnTravail)
                    return false;
            }

            // Vérifier si des nains travaillent
            foreach (Nain nain in Nains)
            {
                if (nain.Statut == StatutEmploye.EnTravail)
                    return false;
            }

            // Vérifier les files d'attente des continents
            foreach (Queue<Lettre> file in FilesAttenteContinents)
            {
                if (file.Count > 0)
                    return false;
            }

            // Vérifier les elfes et traîneaux
            foreach (Elfe elfe in Elfes)
            {
                if (elfe.Traineau.EnVoyage || elfe.Traineau.Lettres.Count > 0)
                    return false;
            }

            return true;
        }

        public void AvancerJusquaJourSuivant()
        {
            int heuresPassees = 0;
            int reste = 12 - (HeureActuelle % 12);
            if (reste == 0) reste = 12; // si on est exactement au début d'une journée, avancer d'une journée complète
            for (int i = 0; i < reste; i++)
            {
                AvancerUneHeure();
                heuresPassees++;
            }
        }
public void ModifierNombreLutins(int nouveauNombre)
{
    if (nouveauNombre < 0 || nouveauNombre > NbLutins)
        throw new ArgumentException("Nombre de lutins invalide.");

    if (HeureActuelle - DerniereModificationLutins < 12)
    {
        Console.WriteLine("Modification impossible avant 12 heures.");
        return;
    }

    // Activer ou désactiver lutins selon nouveauNombre
    int actifs = Lutins.Count(l => l.Statut != StatutEmploye.EnRepos);

    if (nouveauNombre > actifs)
    {
        var lutinsRepos = Lutins.Where(l => l.Statut == StatutEmploye.EnRepos).Take(nouveauNombre - actifs);
        foreach (var lutin in lutinsRepos)
            lutin.Statut = StatutEmploye.EnAttente;
    }
    else if (nouveauNombre < actifs)
    {
        var lutinsActifs = Lutins.Where(l => l.Statut != StatutEmploye.EnRepos).Take(actifs - nouveauNombre);
        foreach (var lutin in lutinsActifs)
            lutin.Statut = StatutEmploye.EnRepos;
    }

    DerniereModificationLutins = HeureActuelle;
}

public void ModifierNombreNains(int nouveauNombre)
{
    if (nouveauNombre < 0 || nouveauNombre > NbNains)
        throw new ArgumentException("Nombre de nains invalide.");

    if (HeureActuelle - DerniereModificationNains < 24)
    {
        Console.WriteLine("Modification impossible avant 24 heures.");
        return;
    }

    int actifs = Nains.Count(n => n.Statut != StatutEmploye.EnRepos);

    if (nouveauNombre > actifs)
    {
        var nainsRepos = Nains.Where(n => n.Statut == StatutEmploye.EnRepos).Take(nouveauNombre - actifs);
        foreach (var nain in nainsRepos)
            nain.Statut = StatutEmploye.EnAttente;
    }
    else if (nouveauNombre < actifs)
    {
        var nainsActifs = Nains.Where(n => n.Statut != StatutEmploye.EnRepos).Take(actifs - nouveauNombre);
        foreach (var nain in nainsActifs)
            nain.Statut = StatutEmploye.EnRepos;
    }

    DerniereModificationNains = HeureActuelle;
}

        public void AfficherIndicateurs()
        {
            Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"║  INDICATEURS - Jour {JourActuel}, Heure {HeureActuelle % 12 + 1}/12");
            Console.WriteLine("╠════════════════════════════════════════════════════════════╣");

            // Lutins - compter manuellement
            int lutinsEnTravail = 0;
            int lutinsEnAttente = 0;
            int lutinsEnRepos = 0;
            foreach (Lutin lutin in Lutins)
            {
                if (lutin.Statut == StatutEmploye.EnTravail)
                    lutinsEnTravail++;
                else if (lutin.Statut == StatutEmploye.EnAttente)
                    lutinsEnAttente++;
                else if (lutin.Statut == StatutEmploye.EnRepos)
                    lutinsEnRepos++;
            }

            Console.WriteLine("║ LUTINS:");
            Console.WriteLine($"║   • En travail: {lutinsEnTravail}");
            Console.WriteLine($"║   • En attente: {lutinsEnAttente}");
            Console.WriteLine($"║   • En repos: {lutinsEnRepos}");
            Console.WriteLine($"║   • Cadeaux en fabrication: {lutinsEnTravail}");
            Console.WriteLine($"║   • Lettres en attente: {FileAttenteFabrication.Count}");
            Console.WriteLine("║ LETTRES EN FABRICATION (lutins) :");
        foreach (var lutin in Lutins)
        {
            if (lutin.Statut == StatutEmploye.EnTravail && lutin.LettreEnCours != null)
            {
                var lettre = lutin.LettreEnCours;
                Console.WriteLine($"║   • Lutin {lutin.Id} : {lettre.Prenom}, {lettre.Adresse}, Age {lettre.Age}");
            }
        }
            // Nains - compter manuellement
            int nainsEnTravail = 0;
            int nainsEnAttente = 0;
            int nainsEnRepos = 0;
            foreach (Nain nain in Nains)
            {
                if (nain.Statut == StatutEmploye.EnTravail)
                    nainsEnTravail++;
                else if (nain.Statut == StatutEmploye.EnAttente)
                    nainsEnAttente++;
                else if (nain.Statut == StatutEmploye.EnRepos)
                    nainsEnRepos++;
            }

            Console.WriteLine("║ NAINS:");
            Console.WriteLine($"║   • En travail: {nainsEnTravail}");
            Console.WriteLine($"║   • En attente: {nainsEnAttente}");
            Console.WriteLine($"║   • En repos: {nainsEnRepos}");
            Console.WriteLine($"║   • Cadeaux en emballage: {nainsEnTravail}");
            Console.WriteLine($"║   • Cadeaux en attente d'emballage: {FileAttenteEmballage.Count}");

            // Afficher lettres en emballage (nains)
            Console.WriteLine("║ LETTRES EN EMBALLAGE (nains) :");
            foreach (var nain in Nains)
            {
                if (nain.Statut == StatutEmploye.EnTravail && nain.LettreEnCours != null)
                {
                    var lettre = nain.LettreEnCours;
                    Console.WriteLine($"║   • Nain {nain.Id} : {lettre.Prenom}, {lettre.Adresse}, Age {lettre.Age}");
                }
            }

            // Elfes et traîneaux
            Console.WriteLine("║ ELFES ET TRAÎNEAUX:");
            foreach (Elfe elfe in Elfes)
            {
                string statut = elfe.Traineau.EnVoyage ?
                    $"En voyage ({elfe.Traineau.HeuresVoyageRestantes}h restantes)" :
                    $"Au chargement ({elfe.Traineau.Lettres.Count}/{NbJouetParTraineau})";
                int enAttente = FilesAttenteContinents[(int)elfe.Continent].Count;
                Console.WriteLine($"║   • {elfe.Continent}: {statut}, En attente: {enAttente}");
            }

            // Coûts - calculer la moyenne manuellement
            Console.WriteLine("║ COÛTS:");
            Console.WriteLine($"║   • Coût total: {CoutTotal:F2} pièces d'or");

            double coutMoyen = 0;
            if (CoutsParHeure.Count > 0)
            {
                double somme = 0;
                foreach (double cout in CoutsParHeure)
                {
                    somme += cout;
                }
                coutMoyen = somme / CoutsParHeure.Count;
            }
            Console.WriteLine($"║   • Coût moyen/heure: {coutMoyen:F2} pièces d'or");

            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
        }

        public void AfficherBilan()
        {
            Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                     BILAN FINAL");
            Console.WriteLine("╠════════════════════════════════════════════════════════════╣");
            Console.WriteLine($"║ Durée totale: {JourActuel - 1} jours et {HeureActuelle % 12} heures");
            Console.WriteLine($"║ Coût total: {CoutTotal:F2} pièces d'or");

            // Calculer la moyenne manuellement
            double coutMoyen = 0;
            if (CoutsParHeure.Count > 0)
            {
                double somme = 0;
                foreach (double cout in CoutsParHeure)
                {
                    somme += cout;
                }
                coutMoyen = somme / CoutsParHeure.Count;
            }
            Console.WriteLine($"║ Coût moyen par heure: {coutMoyen:F2} pièces d'or");

            Console.WriteLine("╠════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ ENTREPÔTS CONTINENTAUX:");

            for (int i = 0; i < Entrepots.Length; i++)
            {
                Entrepot entrepot = Entrepots[i];
                Console.WriteLine($"║ {entrepot.Continent}:");
                Console.WriteLine($"║   Total: {entrepot.Lettres.Count} jouets");
                int[] stats = entrepot.GetStatistiques();

                // Parcourir chaque type de cadeau
                for (int j = 0; j < stats.Length; j++)
                {
                    if (stats[j] > 0)
                    {
                        Cadeau type = (Cadeau)j;
                        Console.WriteLine($"║   • {type}: {stats[j]}");
                    }
                }
            }

            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
        }
    }

    // ============= PROGRAMME PRINCIPAL =============

}
