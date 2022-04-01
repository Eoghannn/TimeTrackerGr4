M1 IMIS 
Rendu du Projet Xamarin.Forms Groupe 4

Membres du groupe : 
- Matysse DESCAMPS
- Eoghann VEAUTE
- Pierre ZACHARY


Cahier des charges :
- [x] Ecran de connexion / inscription 
  - Précision : pour se déconnecter, on passe par la flèche "back" dans la main page, qui nous rammène sur la page de loggin
- [x] Profil Utilisateur éditable
- [x] Edition du mot de passe
- [x] Liste de Projet
  - La liste apparaît sur la page d'accueil dans "Projet en cours"
- [x] Liste de tâches dans chaque projet
  - Pour accéder à la liste de tâches d'un projet, vous pouvez : cliqué sur le titre du projet, ou cliqué sur le bouton "Start" dans la swipeview de l'item représentant le projet
- [x] Historique du temps passé sur chaque projet 
  - Le temps total passé sur une tâche ou un projet est affiché à droite de celui-ci dans leurs listes respectives
- [x] Démarrer un timer depuis une tâche 
  - Pour démarrer un timer on passe par le bouton Start dans la swipeview de la tâche que l'on souhaite
  - Si la tâche est déjà démarrée, on peut alors l'arrêter sur ce même bouton, qui devient alors "Stop" 
  - Le fait de démarrer un timer met en couleur son affichage, pour signifier qu'il est démarré
  - Si une tâche du projet est déjà démarrée, en démarrer une nouvelle stop l'ancienne tâche
  - Il est possible de démarrer plusieurs tâches de différents projets en même temps
- [x] Démarrer un timer depuis la page d'accueil
  - Pour cela, deux possibilités : 
    - Démarrer le timer de la dernière task que l'utilisateur a effectué
    - Démarrer le timer d'un projet, qui ouvre en fait la vue de celui-ci, pour pouvoir démarrer le timer d'une tâche
  - Si une tâche du projet est démarré, il est alors possible de la stopper depuis la swipe view du projet ( le bouton sera alors "Stop" ), sans ouvrir la vue du Project
- [x] Graphique pour voir la répartition du temps passé sur chaque projet
- [x] Graphique pour voir la répartition du temps passé sur chaque tâche d'un projet
  - Pour l'affichage de graphique dans la popup nous avons utilisé le plugin Rg.Plugins.Popup
  - Nous affichons une pie chart avec le plugin Microcharts.Forms
  - Le graphique est vide si le temps passé est de 0 ()
  - Le graphique prend bien les couleures attribuées ( automatiquement ) aux projets

Fonctionnalitées additionnelles : 
- [x] Création  de projets / tâches
  - Il y a une bouton "Nouveau projet" ou "Nouvelle tâche" en bas de leurs listes respectives
- [x] Edition / Suppression du titres des projets / tâches
  - Pour modifier ou supprimer un projet, il faut passé par la swipeview, sur chaque item de leurs listes, où se trouve le bouton start et le bouton edit. 
  - Une fois le mode Edit sélectionné, on peut modifié le titre du projet dans l'entry correspondante, et le supprimer en cliquant sur la croix rouge à droite de cette entry
  - Pour valider une édition , il faut à nouveau passer par le bouton "Edit" dans la swipe view. **Note** : Je n'ai pas trouvé comment s'abonner à l'event "Unfocused" de l'entry, depuis la viewmodel ProjectTask.
- [x] Rafraichissement automatique de l'access token
  - Si, pendant que l'app est lancée, le token va arrivé à expiration, celui ci est automatiquement rafraichie avant son expiration
- [x] Reconnexion automatique avec refresh token
  - En arrivant sur l'app, la première page est la page loggin, quand celle-ci est bien construite, on va regarder dans les prefs si on a un refresh token valide, et dans ce cas, on créer un nouvel access token et on push la main page
- [x] Décompte du timer d'une tâche quand l'application est fermée
  - A l'ouverture de l'app, lors de la connexion à la main page, si une task a été démarrer et n'a jamais été arrêté ( dans ce cas StartDate == EndDate dans l'api ), alors on set EndDate à now, ce qui permet d'obtenir le temps totale passé depuis le lancement du décompte et ce, même si l'app était fermée.
- [ ] Mise en câche des requêtes pour éviter de les refaire trop souvent


L'application a été testé sur :
- Android Tiramisu
- Ios 15


Vous pouvez testé l'application déjà peuplée avec les logs : 
- username : emailgroupe4
- password : groupe4