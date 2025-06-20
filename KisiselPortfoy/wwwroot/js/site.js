// site.js

// Basit fade-in animasyonu
document.addEventListener('DOMContentLoaded', function () {
    document.body.style.opacity = 0;
    setTimeout(() => {
        document.body.style.transition = "opacity 0.7s";
        document.body.style.opacity = 1;
    }, 120);
});

// Buton ripple efekti
document.querySelectorAll('.btn-primary').forEach(btn => {
    btn.addEventListener('click', function (e) {
        let ripple = document.createElement("span");
        ripple.className = "ripple";
        this.appendChild(ripple);
        let rect = this.getBoundingClientRect();
        ripple.style.left = (e.clientX - rect.left) + "px";
        ripple.style.top = (e.clientY - rect.top) + "px";
        setTimeout(() => ripple.remove(), 650);
    });
});


// site.js içerisine ekle
document.addEventListener("DOMContentLoaded", function () {
    const menuBtn = document.getElementById("menu-toggle");
    const navLinks = document.querySelector(".nav-links");
    if (menuBtn && navLinks) {
        menuBtn.addEventListener("click", () => {
            navLinks.classList.toggle("open");
        });
    }
});