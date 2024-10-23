window.fbAsyncInit = function () {
    FB.init({
        appId: '1015689366876643',
        cookie: true,
        xfbml: true,
        version: 'v20.0'
    });

    FB.AppEvents.logPageView();
};

(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) { return; }
    js = d.createElement(s); js.id = id;
    js.src = "https://connect.facebook.net/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));

function checkLoginState() {
    FB.getLoginStatus(function (response) {
        if (response.status === 'connected') {
            // Utente già connesso, procedi con getUserData()
            statusChangeCallback(response);
        } else {
            // Utente non connesso, apri la finestra di dialogo di login di Facebook
            FB.login(function (loginResponse) {
                statusChangeCallback(loginResponse);
            }, { scope: 'email' }); // Aggiungi altre autorizzazioni se necessario
        }
    });
}

function statusChangeCallback(response) {
    if (response.status === 'connected') {
        // Logged into your webpage and Facebook.
        getUserData();
    } else {
        // The person is not logged into your webpage or we are unable to tell.
        console.log('User not authenticated or unable to determine authentication status.');
        // Potresti aggiungere qui delle azioni per gestire il caso in cui l'utente non sia autenticato
    }
}

function getUserData() {
    FB.api('/me', { fields: 'id,name,email' }, function (response) {
        if (response && !response.error) {
            console.log('Successful login for: ' + response.name);
            console.log('Email: ' + response.email);
            console.log('ID: ' + response.id);

            // Chiamata a una funzione per salvare i dati
            saveUserData(response);
        } else {
            console.log('Error fetching user data: ', response.error);
        }
    });
}

function saveUserData(user) {
    fetch('/LogSignInOut/SaveUserData', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            FacebookId: user.id,
            Name: user.name,
            Email: user.email
        })
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            // Gestisci la risposta JSON dal server
            if (data.success) {
                // Operazioni da eseguire se il salvataggio è riuscito
                console.log('Dati salvati con successo.');
                // Posso vedere l'ultimo back?
                window.history.back();
            } else {
                // Operazioni da eseguire se c'è stato un errore di validazione o altro errore sul server
                console.error('Errore durante il salvataggio dei dati:', data.errors);
                // Esempio: mostrare gli errori all'utente o altre azioni di gestione degli errori
            }
        })
        .catch(error => {
            console.error('Errore nella richiesta:', error);
            // Gestione degli errori di rete o altre eccezioni
        });
}
