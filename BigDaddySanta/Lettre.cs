namespace BigDaddySanta
{
    public class Lettre
    {
        public int Id { get; }
        public string Prenom { get; set; }
        public string Adresse { get; set; }
        public int Age { get; set; }
        public Continent Continent { get; set; }

        public Lettre(string prenom, string adresse, int age, int id)
        {
            if (string.IsNullOrWhiteSpace(prenom))
                throw new ArgumentException("Le prénom ne peut pas être vide.");

            if (string.IsNullOrWhiteSpace(adresse))
                throw new ArgumentException("L'adresse ne peut pas être vide.");

            if (age < 0 || age > 18)
                throw new ArgumentException("L'âge doit être entre 0 et 18.");

            Id = id;
            Prenom = prenom;
            Adresse = adresse;
            Age = age;
            Continent = DeterminerContinent(adresse);
        }

        public static Continent RecupererContinent(string continent)
        {
            return continent.Trim().ToLower() switch
            {
                "europe" => Continent.Europe,
                "asie" => Continent.Asie,
                "afrique" => Continent.Afrique,
                "amerique" => Continent.Amerique,
                "oceanie" => Continent.Oceanie,
                _ => throw new ArgumentException("Continent invalide : " + continent)
            };
        }

        public static string[] DecoupageAdresse(string adresse, char delimiteur)
        {
            string[] parties = adresse.Split(delimiteur, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parties.Length; i++)
            {
                parties[i] = parties[i].Trim();
            }
            return parties;
        }

        public static Continent DeterminerContinent(string adresse)
        {
            string[] adresseDecoupee = DecoupageAdresse(adresse, ',');
            if (adresseDecoupee.Length == 0)
                throw new ArgumentException("Adresse vide ou mal formatée");

            string dernierMot = adresseDecoupee[^1];
            return RecupererContinent(dernierMot);
        }

        public static Cadeau RecupererCadeau(int age)
        {
            if (age >= 0 && age <= 2) return Cadeau.Nounours;
            if (age >= 3 && age <= 5) return Cadeau.Tricycle;
            if (age >= 6 && age <= 10) return Cadeau.Jumelles;
            if (age >= 11 && age <= 15) return Cadeau.GeekyJunior;
            if (age >= 16 && age <= 18) return Cadeau.Ordinateur;
            throw new ArgumentException("Age invalide : " + age);
        }

        public static int ObtenirDureeFabrication(int age)
        {
            Cadeau type = RecupererCadeau(age);
            return type switch
            {
                Cadeau.Nounours => 3,
                Cadeau.Tricycle => 4,
                Cadeau.Jumelles => 6,
                Cadeau.GeekyJunior => 1,
                Cadeau.Ordinateur => 10,
                _ => throw new ArgumentException("Type de cadeau invalide")
            };
        }

        public Cadeau Cadeau => RecupererCadeau(Age);

        public int DureeFabrication => ObtenirDureeFabrication(Age);
    }
}
