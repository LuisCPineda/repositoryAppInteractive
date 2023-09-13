using _3GP_TP1;
using System.Collections.Concurrent;
using System.Text;

//Déclaration de variables constantes
const int NB_VOYAGES = 20;
const int NB_TELEPHONISTES = 2;
const int NB_TAXIS = 5;
const int NB_REPARTITEURS = 1;

//Décalation de verrous
object verrouTaxi=new object();
object verrouTelephoniste = new object();
object verrouRepartiteur = new object();

//Déclaration de listes pour sauvegarder liste de voyages génerés par les threads de téléphonistes et
//listes de taxis  génerés par les thread de taxis
List<Voyage> listeVoyages=new List<Voyage>();
List<Taxi> listeTaxi=new List<Taxi>();
//Déclaration de threads téléphonistes, taxis et repartiteur selon les quantité déclarées de variables
//NB_TELEPHONISTES,NB_TAXIS et NB_REPARTITEURS
Thread[] thTelephonistes = new Thread[NB_TELEPHONISTES];
Thread[] thTaxis = new Thread[NB_TAXIS];
Thread[] thRepartiteur = new Thread[NB_REPARTITEURS];
//Déclaration de variable random pour générer le temps aléatoire de création de voyages, et le temps
//aléatoire pour bouger chaque taxi
Random random = new Random();


//Création de chaque thread de taxi selon la méthode CreerTaxis
for(int i = 0; i < thTaxis.Length; i++)
{
    thTaxis[i]= new Thread(new ThreadStart(CreerTaxis));
}
//Initialiser chaque thread de taxi
foreach(Thread th in thTaxis)
{
    th.Start();
}

//Création de chaque thread de chaque téléphoniste selon la méthode CreerVoyages
for (int i = 0; i < thTelephonistes.Length; i++)
{
    thTelephonistes[i] = new Thread(new ThreadStart(CreerVoyages));
}
//Initialiser chaque thread de téléphoniste
foreach (Thread th in thTelephonistes)
{
    th.Start();
}

//Création de chaque thread de chaque répartiteur selon la méthode AssignerVoyage
for (int i = 0; i < thRepartiteur.Length; i++)
{
    thRepartiteur[i]=new Thread(new ThreadStart(AssignerVoyage));
}

//Initialiser chaque thread de répartiteur
foreach (Thread th in thRepartiteur)
{
    th.Start();
}

//Bucles pour arrêter chaque thread des thTelephonistes,thTaxis et thRepartiteur
foreach (Thread th in thTelephonistes)
{
    th.Join();
}
foreach (Thread th in thTaxis)
{
    th.Join();

}
foreach (Thread th in thRepartiteur)
{
    th.Join();
}

//Méthodes pour creer taxis selon le nombre de threads de taxis et pour vérifier si le taxi
// est disponible afin de commencer à bouger chaque taxi selon les coordonnées assignées au 
// point de départ vers le point d'arrivée
void CreerTaxis()
{
    //Variable listeVoyageNonVide pour vérifier si la liste de voyage est vide.
    bool listeVoyageNonVide =true;
    //Verrou pour la bloque la création de voyage
    lock (verrouTaxi)
    {
        Taxi nouveauTaxi = new Taxi();
        listeTaxi.Add(nouveauTaxi);
        Console.WriteLine(nouveauTaxi.StringPositionTaxi());
    }
    //On arrete la méthode pour que la méthode de génération de voyage puisse mettre de voyages
    //dans la liste de voyage
    Thread.Sleep(4050);
    //Boucle pour vérifier s'il y a voyage dans la liste de voyage
    while (listeVoyageNonVide)
    {
        lock (verrouTaxi)
        {
            //Boucle pour vérifier s'il y a taxis avec voyage assignés pour les bouger 
            foreach (Taxi taxi in listeTaxi)
            {
                //Condition pour vérifier si chaque taxi est null
                Thread.Sleep(10);
                if (taxi!=null)
                {
                    //Condition pour vérifier si le taxi est disponible
                    if (!taxi.EtatTaxi)
                    {
                        //Vérification de l'assignation de voyage de chaque taxi
                        if (!taxi.VoyageAssigne.VoyageAuPointDepart)
                        {
                            //Méthode pour bouger le taxi vers le point de départ
                            BougerTaxiAuDepart(taxi);
                        }
                        //Condition pour vérifier si chaque taxi a effectué le voyage jusqu'à le départ
                        if (!taxi.VoyageAssigne.VoyageEffectue && taxi.VoyageAssigne.VoyageAuPointDepart)
                        {
                            //Méthode pour bouger le taxi ver le point d'arrivée
                            BougerTaxiAuArrive(taxi);
                        }
                        //Méthode qui verifie si le taxi se trouve au point d'arrivée et après la méthode
                        //efface le voyage de la liste de voyage
                        EffacerVoyage(taxi);
                    }
                }
                
            }
        }
        //Méthode pour vérifier si la liste de voyage est vide est changer la variable listeVoyageNonVide et finaliser 
        //le boucle 
        if (listeVoyages.Count == 0)
        {
            listeVoyageNonVide = false;
        }
    }
}
//Méthode pour creer le nombre de voyage définés par la variable NB_VOYAGES
void CreerVoyages()
{
    for(int i = 0; i < NB_VOYAGES; i++)
    {
        lock (verrouTelephoniste)
        {
            int tempsRepos = random.Next(1000) + 1000;
            Thread.Sleep(tempsRepos);
            Voyage nouveauVoyaye = new Voyage();
            Console.WriteLine(nouveauVoyaye.AfficherDonneeVoyage());
            listeVoyages.Add(nouveauVoyaye);   
        }   
    }
}
//Méthode pour assigner les voyages aux taxis disponibles 
void AssignerVoyage()
{
    //Variable listeVoyageNonVide pour vérifier si la liste de voyage est vide.
    bool listeVoyageNonVide = true;
    //Boucle pour vérifier s'il y a voyage dans la liste de voyage
    while (listeVoyageNonVide)
    {
        lock (verrouRepartiteur)
        {
            //On arrete la méthode pour que la méthode de génération de voyage puisse mettre de voyages
            //dans la liste de voyage
            Thread.Sleep(4000);
            //Création d'un objet Voyage pour effectuer une comparaison de tous les voyages de la liste
            //de voyages
            Voyage voyagePlusProche = null;

            foreach(Taxi taxi in listeTaxi)
            {
                //Compteur pour prendre le premier voyage de la liste de voyage
                int compteur = 0;
                //Vérification de disponibilité des taxis
                if (taxi.EtatTaxi)
                {
                    lock (verrouTelephoniste)
                    {
                        //Boucle pour chercher le voyage plus proche à chaque taxi
                        foreach (Voyage voyage in listeVoyages)
                        {
                            //Vérification si le voyage a été déjà pris par un taxi 
                            if (!voyage.voyagePrisParTaxi)
                            {
                                //Condition prendre le premier voyage pour effectuer le comparaison
                                if (compteur == 0)
                                {
                                    voyagePlusProche = voyage;
                                }

                                //Delta de comparaison du voyage plus proche avec le voyage suivante de la liste
                                int deltaTaxiPlusProche = ComparerCoordonnees(taxi, voyagePlusProche);
                                int deltaTaxi = ComparerCoordonnees(taxi, voyage);
                                //Condition si le delta d'un voyage est superieur au delta de de chaque voyage
                                if (deltaTaxiPlusProche >= deltaTaxi)
                                {
                                    voyagePlusProche = voyage;
                                }
                                compteur++;
                            }

                        }
                        //Après d'avoir trouvé le voyage plus proche à chaque taxi, cette boucle effectue l'assignation 
                        //du voyage au taxi
                        foreach (Voyage voyage in listeVoyages)
                        {
                            if (voyagePlusProche != null)
                            {
                                if (voyage.NbVoyage == voyagePlusProche.NbVoyage)
                                {
                                    taxi.EtatTaxi = false;
                                    taxi.VoyageAssigne = voyage;
                                    voyage.voyagePrisParTaxi = true;
                                    Console.WriteLine(taxi.StringAssignationTaxi());
                                }
                            }
                            
                        }
                        voyagePlusProche = null;
                        compteur = 0;
                        Thread.Sleep(100);
                    }  
                } 
            }
        }
        //Méthode pour vérifier si la liste de voyage est vide est changer la variable listeVoyageNonVide et finaliser 
        //le boucle 
        if (listeVoyages.Count == 0)
        {
            listeVoyageNonVide = false;
        }
    }
}
//Cette méthode génére le delta entre le coordonnée de taxi et le coordonnée de depart ou d'arrivée
int ComparerCoordonnees(Taxi unTaxi,Voyage unVoyage)
{
    int delta;
    delta=Math.Abs(unTaxi.CoordonneesTaxi.x-unVoyage.coordonneeVoyageDepart.x)+Math.Abs(unTaxi.CoordonneesTaxi.y-unVoyage.coordonneeVoyageDepart.y);
    return delta;
}
//Cette méthode effectue le changement de coordonée d'un taxi vers le point de depart
void BougerTaxiAuDepart(Taxi untaxi)
{
    Coordonnee depart = untaxi.VoyageAssigne.coordonneeVoyageDepart;
    int tempsRepos = random.Next(250) + 250;

    Thread.Sleep(tempsRepos);
    //Verification de coordonnée du taxi dans l'axe x 
    if (untaxi.CoordonneesTaxi.x != depart.x)
    {
        if (untaxi.CoordonneesTaxi.x < depart.x)
        {
            untaxi.CoordonneesTaxi.x++;
        }
        if (untaxi.CoordonneesTaxi.x > depart.x)
        {
            untaxi.CoordonneesTaxi.x--;
        }
    }
    //Verification de coordonnée du taxi dans l'axe y
    if (untaxi.CoordonneesTaxi.y != depart.y)
    {
        if (untaxi.CoordonneesTaxi.y < depart.y)
        {
            untaxi.CoordonneesTaxi.y++;
        }
        if (untaxi.CoordonneesTaxi.y > depart.y)
        {
            untaxi.CoordonneesTaxi.y--;
        }
    }
    //changement de la variable du voyage VoyageAuPointDepart si le taxi arrive au
    //point de depart
    if (untaxi.CoordonneesTaxi.x == depart.x && untaxi.CoordonneesTaxi.y == depart.y)
    {
        Console.WriteLine(untaxi.StringTaxiArriveDepart());
        untaxi.VoyageAssigne.VoyageAuPointDepart = true;
    }
}
//Cette méthode effectue le changement de coordonée d'un taxi vers le point d'arrivée
void BougerTaxiAuArrive(Taxi unTaxi)
{
    Coordonnee arrive = unTaxi.VoyageAssigne.coordonneeVoyageArrivee;
    int tempsRepos = random.Next(250) + 250;

    Thread.Sleep(tempsRepos);
    //Verification de coordonnée du taxi dans l'axe x 
    if (unTaxi.CoordonneesTaxi.x != arrive.x)
    {
        if (unTaxi.CoordonneesTaxi.x < arrive.x)
        {
            unTaxi.CoordonneesTaxi.x++;
        }
        if (unTaxi.CoordonneesTaxi.x > arrive.x)
        {
            unTaxi.CoordonneesTaxi.x--;
        }
    }
    //Verification de coordonnée du taxi dans l'axe y
    if (unTaxi.CoordonneesTaxi.y != arrive.y)
    {
        if (unTaxi.CoordonneesTaxi.y < arrive.y)
        {
            unTaxi.CoordonneesTaxi.y++;
        }
        if (unTaxi.CoordonneesTaxi.y > arrive.y)
        {
            unTaxi.CoordonneesTaxi.y--;
        }
    }
    //changement de la variable du voyage VoyageAuPointDepart si le taxi arrive au
    //point d'arrivée
    if (unTaxi.CoordonneesTaxi.x == arrive.x && unTaxi.CoordonneesTaxi.y == arrive.y)
    {
        unTaxi.VoyageAssigne.VoyageEffectue = true;
        unTaxi.EtatTaxi = true;
        Console.WriteLine(unTaxi.StringTaxiArriveFinCours());
    }
}
//Méthode qui verifie si le voyage a été effectue et l'efface si c'est le cas 
void EffacerVoyage(Taxi unTaxi)
{
    Thread.Sleep(10);

    if (unTaxi.VoyageAssigne.VoyageEffectue)
    {   
        if (unTaxi.VoyageAssigne.NbVoyage > 0)
        {
            lock (verrouTelephoniste)
            {
                for(int j=0; j < listeVoyages.Count; j++)
                {
                    if (unTaxi.VoyageAssigne.NbVoyage == listeVoyages[j].NbVoyage)
                    {
                        listeVoyages.Remove(listeVoyages[j]);
                        j = NB_TELEPHONISTES * NB_TELEPHONISTES;
                        Console.WriteLine("Il reste "+listeVoyages.Count+" voyages.");
                    }
                }
            }
        }
        unTaxi.VoyageAssigne = null;
    }
}



