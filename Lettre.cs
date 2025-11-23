using System.ComponentModel.Design;
using System.Dynamic;
using System.Reflection.Metadata.Ecma335;

public enum Cadeau
{
    Nounours,
    Tricycle,
    Jumelles,
    GeekyJunior,
    Ordinateur,
}

public enum Continent
{
    Europe,
    Asie,
    Afrique,
    Amerique,
    Oceanie,
}

public class Lettre
{
    public string Prenom { get; set; }
    public string Adresse { get; set; }
    public int Age { get; set; }


    public Lettre(string prenom, string adresse, int age)
    {
        Prenom = prenom;
        Adresse = adresse;
        Age = age;
    }

    public static Continent RecupererContinent(string continent)
    {
        switch (continent)
        {
            case "Europe":
                return Continent.Europe;
            case "Asie":
                return Continent.Asie;
            case "Afrique":
                return Continent.Afrique;
            case "Amerique":
                return Continent.Amerique;
            case "Oceanie":
                return Continent.Oceanie;
            default:
                throw new ArgumentException("Continent Invalide : ", nameof(continent));
        }
    }

    public static string[] DecoupageAdresse(string adresse, char delimiteur, int tailleTableau)
    //J'ai mis le paramètre tailleTableau parce que je ne connais pas encore la convention de l'adresse
    //Paramètre à mettre en constante ou en variable dans la classe Lettre
    {
        string[] tableauAdresse = new string[tailleTableau];
        string motCourant = "";
        int compteur = 0;
        for (int i = 0; i < adresse.Length; i++)
        //une chaine de caractère est un tableau de chaine de caractères contenant chaque lettre du mot
        {
            char lettreCourante = adresse[i];
            if (i == adresse.Length - 1)
            {
                //le compilateur effectue implicitement la conversion du type char en type string pour produire la concaténation
                motCourant += lettreCourante; //Equivalent à motCourant = motCourant + lettreCourante
                tableauAdresse[compteur] = motCourant;
            }
            else if (lettreCourante == delimiteur)
            {
                tableauAdresse[compteur] = motCourant;
                motCourant = "";
                compteur++; //Equivalent à compteur = compteur + 1
            }
            else if (lettreCourante != ' ' && motCourant == "" || motCourant != "")
            //Je veux retirer les espaces au début de la chaine de caractère séparé par le délimiteur, 
            //la condition pour former le mot sans cet espace c'est que lettreCourante soit différent du caractère espace et que motCourant soit une chaine vide.
            //Cela nous assure que c'est bien le début d'un nouveau mot, ensuite il faut bien évidemment récupérer les autres lettres,
            //même si ce sont des espaces, pour ce faire on utilise la condition du motCourant != "",
            // ce qui nous permet de savoir qu'on est plus dans le premier cas.
            {
                motCourant += lettreCourante;
            }
            
        }
        return tableauAdresse;
    }

    public static Continent DeterminerContinent(string adresse, int tailleTableau) 
    {
        string[] adresseDecoupee = DecoupageAdresse(adresse, ',', tailleTableau); //Il faut changer taille adresse pour mettre une variable au lieu d'une constante
        int tailleAdresseDecoupe = adresseDecoupee.Length;
        return RecupererContinent(adresseDecoupee[tailleAdresseDecoupe - 1]);
    }

    public static Cadeau RecupererCadeau(int age)
    {
        if (age >= 0 && age <= 2) return Cadeau.Nounours;
        else if (age >= 3 && age <= 5) return Cadeau.Tricycle;
        else if (age >= 6 && age <= 10) return Cadeau.Jumelles;
        else if (age >= 11 && age <= 15) return Cadeau.GeekyJunior;
        else if (age >= 16 && age <= 18) return Cadeau.Ordinateur;
        else throw new ArgumentException("Age Invalide", nameof(age));
    }

    public static int ObtenirDureeFabrication(int age)
    {
        Cadeau type = RecupererCadeau(age);
        switch (type)
        {
            case Cadeau.Nounours:
                return 3;
            case Cadeau.Tricycle:
                return 4;
            case Cadeau.Jumelles:
                return 6;
            case Cadeau.GeekyJunior:
                return 1;
            case Cadeau.Ordinateur:
                return 10;
            default:
                throw new ArgumentException("Type Invalide", nameof(type));
        }
    }

    public Cadeau Cadeau
    {
        get
        {
            return RecupererCadeau(Age);
        }
    }

    public int DureeFabricant
    {
        get
        {
            return ObtenirDureeFabrication(Age);
        }
    }
}
