document.addEventListener('DOMContentLoaded', function () {
    console.log('DOMContentLoaded event triggered');

    document.getElementById('showEditModalBtn').addEventListener('click', function () {
        console.log('showEditModalBtn clicked');
        document.getElementById('editModal').style.display = 'block';
    });

    document.getElementById('showDetailModalBtn').addEventListener('click', function () {
        console.log('showDetailModalBtn clicked');
        document.getElementById('detailModal').style.display = 'block';
    });

    document.getElementById('closeBtn').addEventListener('click', function () {
        console.log('closeBtn clicked');
        document.getElementById('editModal').style.display = 'none';
        document.getElementById('detailModal').style.display = 'none';
    });

    document.getElementById('mainCloseBtn').addEventListener('click', function () {
        console.log('mainCloseBtn clicked');
        document.getElementById('editModal').style.display = 'none';
        document.getElementById('detailModal').style.display = 'none';
    });

    window.addEventListener('click', function (event) {
        console.log('window clicked');
        if (event.target == document.getElementById('editModal')) {
            document.getElementById('editModal').style.display = 'none';
        }
        if (event.target == document.getElementById('detailModal')) {
            document.getElementById('detailModal').style.display = 'none';
        }
    });
});


function openEditModal(userId, fname, mi, lname, department, dob, phone, address, email) {

    document.getElementById('editModal').innerHTML = `
        <div class="modal-content">
            <i class='bx bx-arrow-back' onclick="closeModal('editModal')" style="font-size: 35px !important;"></i>
            <div class="centered-text">
                <h1>Edit Staff</h1>
            </div>
            <div class="form-container">
                <div class="form-group">
                    <label for="Firstname">Firstname:</label>
                    <input type="text" id="Firstname" name="Firstname" class="textbox-style" style="width: 300px !important;" value="${fname}" required autofocus />
                </div>
                <div class="form-group">
                    <label for="MI">M.I:</label>
                    <input type="text" id="MI" name="MI" class="textbox-style" style="width: 300px !important;" value="${mi}" required />
                </div>
                <div class="form-group">
                    <label for="Lastname">Lastname:</label>
                    <input type="text" id="Lastname" name="Lastname" class="textbox-style" style="width: 300px !important;" value="${lname}" required />
                </div>

                <div class="form-group">
                    <label for="Phone">Phone:</label>
                    <input type="text" id="Phone" name="Phone" class="textbox-style" style="width: 300px !important;" value="${phone}" required />
                </div>

                <div class="form-group">
                    <label for="Address">Address:</label>
                    <input type="text" id="Address" name="Address" class="textbox-style" style="width: 300px !important;" value="${address}" required />
                </div>

                 <div class="form-group">
                    <label for="Email">Email:</label>
                    <input type="text" id="Email" name="Email" class="textbox-style" style="width: 300px !important;" value="${email}" required />
                </div>

              

             

                <!-- Add more fields as needed -->
            </div>

            <!-- Add your form or other content for editing staff here -->
            </br></br>
       <i class='bx bxs-edit-alt' onclick="editStaff('${userId}')" style="margin-left: 1000px; font-size: 30px !important;"></i>



        </div>
    `;

    // Show the modal
    document.getElementById('editModal').style.display = 'block';
}

function editStaff(userId) {
    // Redirect to the edit action on the server
    window.location.href = `/Staff/EditStaff/${userId}`;
}


function openDetailModal(userId, fname, mi, lname, department, status) {
    // Update modal content based on the provided details
    document.getElementById('detailModal').innerHTML = `
        <div class="modal-content">
            <i class='bx bx-arrow-back' onclick="closeModal('detailModal')" style="font-size: 35px !important;"></i>
            <div class="centered-text">
                <h1>Staff Details</h1>
            </div>
            <div class="details-container">
                <p><strong>Firstname:</strong> ${fname}</p>
                <p><strong>M.I:</strong> ${mi}</p>
                <p><strong>Lastname:</strong> ${lname}</p>
                <p><strong>Department:</strong>${department}</p>
                <!-- Add more fields as needed -->
            </div>

            <!-- Add any additional content for viewing details -->
        </div>
    `;

    // Show the modal
    document.getElementById('detailModal').style.display = 'block';
}



function closeModal(modalId) {
    // Hide the specified modal
    document.getElementById(modalId).style.display = 'none';
}