async function getProfileData() {
    try {
        // Show loading state
        showLoadingState();

        // Get user ID from hidden span
        let id = document.getElementById("idspan").innerText.trim();

        if (!id) {
            console.error("No user ID found");
            showErrorState();
            return;
        }

        console.log("Fetching profile for user ID:", id);

        // Make API request
        const response = await fetch(`https://localhost:7224/api/Profile?id=${id}`, {
            method: 'GET',
            credentials: "include",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            console.error("Request failed with status:", response.status);
            showErrorState();
            return;
        }

        const data = await response.json();
        console.log("Profile data received:", data);

        // Update DOM with profile data
        updateProfileDOM(data);

        // Show profile content
        showProfileContent();

    } catch (error) {
        console.error("Error while fetching profile data:", error);
        showErrorState();
    }
}

function updateProfileDOM(profileData) {
    try {
        // Update profile image
        const profileImage = document.getElementById("profileImage");
        const profileImagePlaceholder = document.getElementById("profileImagePlaceholder");

        if (profileData.picURL && profileData.picURL.trim() !== "") {
            profileImage.src = profileData.picURL;
            profileImage.style.display = "block";
            profileImagePlaceholder.style.display = "none";
        } else {
            profileImage.style.display = "none";
            profileImagePlaceholder.style.display = "flex";
        }

        // Update full name in header
        const fullName = `${profileData.firstName || ''} ${profileData.lastName || ''}`.trim();
        document.getElementById("profileFullName").textContent = fullName || "No Name Available";

        // Update job title in header
        const jobTitle = profileData.jopTitle || "No Job Title";
        document.getElementById("profileJobTitle").textContent = jobTitle;

        // Update individual fields
        document.getElementById("profileFirstName").textContent = profileData.firstName || "Not specified";
        document.getElementById("profileLastName").textContent = profileData.lastName || "Not specified";
        document.getElementById("profileJobTitleDetail").textContent = profileData.jopTitle || "Not specified";
        document.getElementById("profileId").textContent = profileData.id || "Unknown";

        // Update edit button link
        const editBtn = document.getElementById("editProfileBtn");
        if (editBtn && profileData.id) {
            const currentHref = editBtn.getAttribute('href') || editBtn.getAttribute('asp-route-Id');
            editBtn.href = editBtn.href.replace(/Id=\d*/, `Id=${profileData.id}`);
        }

        console.log("DOM updated successfully");

    } catch (error) {
        console.error("Error updating DOM:", error);
        showErrorState();
    }
}

function showLoadingState() {
    document.getElementById("loadingState").style.display = "block";
    document.getElementById("errorState").style.display = "none";
    document.getElementById("profileContent").style.display = "none";
}

function showErrorState() {
    document.getElementById("loadingState").style.display = "none";
    document.getElementById("errorState").style.display = "block";
    document.getElementById("profileContent").style.display = "none";
}

function showProfileContent() {
    document.getElementById("loadingState").style.display = "none";
    document.getElementById("errorState").style.display = "none";
    document.getElementById("profileContent").style.display = "block";
}

function retryLoadProfile() {
    console.log("Retrying profile load...");
    getProfileData();
}

// Initialize when page loads
document.addEventListener('DOMContentLoaded', function () {
    console.log("Profile page loaded, fetching data...");
    getProfileData();
});

// Also call immediately for compatibility
getProfileData();
console.log("Profile script loaded");