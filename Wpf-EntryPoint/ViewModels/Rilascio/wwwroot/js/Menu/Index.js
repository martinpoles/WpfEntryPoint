export function initMenu() {

    const footer = document.querySelector('footer');
    if (footer) {
        footer.style.display = 'none';
    }

    document.body.style.transition = "background-image 0.5s ease-in-out";
    document.body.style.backgroundImage = "url('/Img/BG_Img.webp')";
    document.body.style.backgroundSize = "cover";

}