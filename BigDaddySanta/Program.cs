namespace BigDaddySanta
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Simulation simulation = null;
            bool continuer = true;

            while (continuer)
            {
                if (simulation == null)
                {
                    simulation = MenuInitialisation();
                }
                else
                {
                    continuer = MenuPrincipal(simulation);

                    if (continuer && simulation.ToutesLettresTraitees())
                    {
                        Console.WriteLine("\n🎄 Toutes les lettres ont été traitées! 🎄");
                        simulation.AfficherBilan();

                        Console.Write("\nVoulez-vous lancer une nouvelle simulation? (o/n): ");
                        string choix = Console.ReadLine().ToLower();
                        if (choix == "o" || choix == "oui")
                        {
                            simulation = null;
                        }
                        else
                        {
                            continuer = false;
                        }
                    }
                }
            }

            Console.WriteLine("\n🎅 Joyeux Noël! 🎄");
        }

        static Simulation MenuInitialisation()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║          BIENVENUE AU PAYS DU PÈRE NOËL                    ║");
            Console.WriteLine("║              Initialisation de la simulation               ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            Console.Write("Nombre de lutins disponibles: ");
            int nbLutins = int.Parse(Console.ReadLine());

            Console.Write("Nombre de nains disponibles: ");
            int nbNains = int.Parse(Console.ReadLine());

            Console.Write("Nombre d'enfants: ");
            int nbEnfants = int.Parse(Console.ReadLine());

            Console.Write("Nombre maximum de lettres par heure: ");
            int nbLettresParHeure = int.Parse(Console.ReadLine());

            Console.Write("Capacité de chaque traîneau: ");
            int nbJouetParTraineau = int.Parse(Console.ReadLine());

            Console.WriteLine("\n✅ Simulation initialisée avec succès!");
            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();

            return new Simulation(nbLutins, nbNains, nbEnfants, nbLettresParHeure, nbJouetParTraineau);
        }

        static bool MenuPrincipal(Simulation simulation)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                 GESTION DU PÈRE NOËL                       ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ 1. Avancer d'une heure                                     ║");
            Console.WriteLine("║ 2. Avancer jusqu'au début de la journée suivante           ║");
            Console.WriteLine("║ 3. Afficher les indicateurs                                ║");
            Console.WriteLine("║ 4. Gérer les lutins                                        ║");
            Console.WriteLine("║ 5. Gérer les nains                                         ║");
            Console.WriteLine("║ 6. Visualiser les entrepôts                                ║");
            Console.WriteLine("║ 7. Afficher le bilan et continuer                          ║");
            Console.WriteLine("║ 8. Afficher le bilan et arrêter                            ║");
            Console.WriteLine("║ 9. Afficher le bilan et relancer                           ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.Write("\nVotre choix: ");

            string choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    simulation.AvancerUneHeure();   // Cette Méthode est à écrire.
                    Console.WriteLine($"\n✅ Avancé à: Jour {simulation.JourActuel}, Heure {simulation.HeureActuelle % 12 + 1}");
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return true;

                case "2":
                    int heuresAvant = simulation.HeureActuelle;
                    simulation.AvancerJusquaJourSuivant();  // Cette Méthode est à écrire.
                    Console.WriteLine($"\n✅ Avancé jusqu'à: Jour {simulation.JourActuel}, Heure 1");
                    Console.WriteLine($"   ({simulation.HeureActuelle - heuresAvant} heures écoulées)");
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return true;

                case "3":
                    simulation.AfficherIndicateurs();
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return true;

                case "4":
                    MenuGestionLutins(simulation);
                    return true;

                case "5":
                    MenuGestionNains(simulation);
                    return true;

                case "6":
                    AfficherEntrepots(simulation);
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return true;

                case "7":
                    simulation.AfficherBilan();
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return true;

                case "8":
                    simulation.AfficherBilan();
                    return false;

                case "9":
                    simulation.AfficherBilan();
                    Console.WriteLine("\n🔄 Redémarrage de la simulation...");
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return false;

                default:
                    Console.WriteLine("❌ Choix invalide.");
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return true;
            }
        }

        static void MenuGestionLutins(Simulation simulation)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                   GESTION DES LUTINS                       ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            // Compter les lutins actifs
            int actifs = 0;
            foreach (Lutin lutin in simulation.Lutins)
            {
                if (lutin.Statut != StatutEmploye.EnRepos)
                    actifs++;
            }

            Console.WriteLine($"Lutins actuellement actifs: {actifs}/{simulation.NbLutins}");
            Console.WriteLine($"Dernière modification: il y a {simulation.HeureActuelle - simulation.DerniereModificationLutins} heures");
            Console.WriteLine($"(Modification possible après 12 heures)\n");

            Console.Write("Nouveau nombre de lutins actifs (ou 'c' pour annuler): ");
            string input = Console.ReadLine();

            if (input.ToLower() != "c")
            {
                if (int.TryParse(input, out int nouveau))
                {
                    simulation.ModifierNombreLutins(nouveau);   // Cette Méthode est à écrire.
                }
                else
                {
                    Console.WriteLine("❌ Nombre invalide.");
                }
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        static void MenuGestionNains(Simulation simulation)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                   GESTION DES NAINS                        ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            // Compter les nains actifs
            int actifs = 0;
            foreach (Nain nain in simulation.Nains)
            {
                if (nain.Statut != StatutEmploye.EnRepos)
                    actifs++;
            }

            Console.WriteLine($"Nains actuellement actifs: {actifs}/{simulation.NbNains}");
            Console.WriteLine($"Dernière modification: il y a {simulation.HeureActuelle - simulation.DerniereModificationNains} heures");
            Console.WriteLine($"(Modification possible après 24 heures)\n");

            Console.Write("Nouveau nombre de nains actifs (ou 'c' pour annuler): ");
            string input = Console.ReadLine();

            if (input.ToLower() != "c")
            {
                if (int.TryParse(input, out int nouveau))
                {
                    simulation.ModifierNombreNains(nouveau);    // Cette Méthode est à écrire.
                }
                else
                {
                    Console.WriteLine("❌ Nombre invalide.");
                }
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        static void AfficherEntrepots(Simulation simulation)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║              ÉTAT DES ENTREPÔTS CONTINENTAUX               ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            for (int i = 0; i < simulation.Entrepots.Length; i++)
            {
                Entrepot entrepot = simulation.Entrepots[i];
                Console.WriteLine($"═══ {entrepot.Continent} ═══");
                Console.WriteLine($"Total de jouets stockés: {entrepot.Lettres.Count}");

                int[] stats = entrepot.GetStatistiques();
                Console.WriteLine("Répartition par catégorie d'âge:");

                // Parcourir les types de cadeaux dans l'ordre de l'enum
                foreach (Cadeau type in Enum.GetValues(typeof(Cadeau)))
                {
                    int index = (int)type;
                    if (stats[index] > 0)
                    {
                        string categorie;
                        switch (type)
                        {
                            case Cadeau.Nounours:
                                categorie = "0-2 ans (Nounours)";
                                break;
                            case Cadeau.Tricycle:
                                categorie = "3-5 ans (Tricycle)";
                                break;
                            case Cadeau.Jumelles:
                                categorie = "6-10 ans (Jumelles)";
                                break;
                            case Cadeau.GeekyJunior:
                                categorie = "11-15 ans (Abonnement)";
                                break;
                            case Cadeau.Ordinateur:
                                categorie = "16-18 ans (Ordinateur)";
                                break;
                            default:
                                categorie = "Inconnu";
                                break;
                        }
                        Console.WriteLine($"  • {categorie}: {stats[index]}");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}