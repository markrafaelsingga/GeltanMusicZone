document.addEventListener("DOMContentLoaded", function () {
    const sidebar = document.querySelector(".sidebar");
    const sidebarToggle = document.querySelector(".bx-menu");
    const homeSection = document.querySelector(".home-section");

    sidebarToggle.addEventListener("click", () => {
        sidebar.classList.toggle("close");
        homeSection.classList.toggle("close");

        // Hide all dropdown menus when sidebar is closed
        if (sidebar.classList.contains("close")) {
            const subMenus = document.querySelectorAll(".sub-menu");
            subMenus.forEach((subMenu) => {
                subMenu.classList.remove("showMenu");
            });
        }
    });

    // Toggle dropdown menu when clicking on items with sub-menus
    const subMenuLinks = document.querySelectorAll(".iocn-link");
    subMenuLinks.forEach((link) => {
        link.addEventListener("click", (e) => {
            const subMenu = link.nextElementSibling;
            subMenu.classList.toggle("showMenu");
            const parentLi = link.parentElement;

            // Close other open sub-menus
            subMenuLinks.forEach((otherLink) => {
                if (otherLink !== link) {
                    otherLink.nextElementSibling.classList.remove("showMenu");
                }
            });

            // Toggle arrow rotation
            parentLi.classList.toggle("showMenu");
        });
    });

    // Hide dropdown menus when mouse leaves the parent item
    const navLinks = document.querySelectorAll(".nav-links li");
    navLinks.forEach((navLink) => {
        navLink.addEventListener("mouseleave", () => {
            const subMenu = navLink.querySelector(".sub-menu");
            if (subMenu) {
                subMenu.classList.remove("showMenu");
            }
        });
    });
});
