import { initMenu } from '../Menu/Index.js';
import { initHome } from '../Home/Index.js';
import { initReels } from '../Reels/Index.js';
import { initLazyLoad } from './_lazyLoad.js';

document.addEventListener('DOMContentLoaded', function () {
    // Trova la div con l'attributo personalizzato
    const pageInfoDiv = document.querySelector('#page-info');
    const pageType = pageInfoDiv ? pageInfoDiv.getAttribute('pagina') : '';
    
    switch (pageType) {
        case 'Menu':
            console.log("initMenu");
            initMenu();
            break;
        case 'Index':
            console.log("initHome");
            initHome();
            break;
        case 'Reels':
            console.log("initReels");
            initReels();
            break;
    }

    initLazyLoad();

    // Rimuovo l'img di caricamento
    const loader = document.getElementById('loading-spinner');
    const content = document.getElementById('main-corpo');

    if (loader) loader.style.display = 'none';
    if (content) content.style.display = 'flex';

});
