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


function openEditModal(userId, fname, mi, lname, department, dob, phone, address, email, depList) {
    document.getElementById('editModal').innerHTML = `
        <div class="modal-content">
            <i class='bx bx-arrow-back' onclick="closeModal('editModal')" style="font-size: 35px !important;"></i>
            <div class="centered-text">
                <h1>Edit Staff</h1>
            </div>
            <div class="form-container">
                <div class="form-group">
                    <label for="userId">ID:</label>
                    <input type="text" id="userId" name="userId" class="textbox-style" style="width: 300px !important;" value="${userId}" required autofocus />
                </div>
                <div class="form-group">
                    <label for="fname">Firstname:</label>
                    <input type="text" id="fname" name="fname" class="textbox-style" style="width: 300px !important;" value="${fname}" required autofocus />
                </div>
                <div class="form-group">
                    <label for="mi">M.I:</label>
                    <input type="text" id="mi" name="mi" class="textbox-style" style="width: 300px !important;" value="${mi}" required />
                </div>
                <div class="form-group">
                    <label for="lname">Lastname:</label>
                    <input type="text" id="lname" name="lname" class="textbox-style" style="width: 300px !important;" value="${lname}" required />
                </div>
                <div class="form-group">
                    <label for="DepartmentLabel">Department:</label>
                    <input type="text" id="Department" name="Department" class="textbox-style" style="width: 300px !important;" value="${department}" required autofocus />
                </div>

                <div class="form-group">
                    <label for="phone">Phone:</label>
                    <input type="text" id="phone" name="phone" class="textbox-style" style="width: 300px !important;" value="${phone}" required />
                </div>

                <div class="form-group">
                    <label for="address">Address:</label>
                    <input type="text" id="address" name="address" class="textbox-style" style="width: 300px !important;" value="${address}" required />
                </div>

                <div class="form-group">
                    <label for="email">Email:</label>
                    <input type="text" id="email" name="email" class="textbox-style" style="width: 300px !important;" value="${email}" required />
                </div>
                
                <div class="form-group">
                    <label for="DepartmentSelect">Department:</label>
                    <select asp-for="depId" class="form-control">
                        <option selected disabled>${department}</option>
                        @foreach (var dep in ViewBag.DepList)
                        {
                            <option value="@dep.Value">@dep.Text</option>
                        }
                    </select>
                    <span asp-validation-for="depId" class="text-danger"></span>
                </div>
            </div>

            <i class='bx bxs-edit-alt' onclick="editStaff(${userId}, '${fname}', '${mi}', '${lname}', '${phone}', '${address}', '${email}')" style="margin-left: 1000px; font-size: 30px !important;"></i>
            </br></br>
        </div>
    `;

    // Show the modal
    document.getElementById('editModal').style.display = 'block';
}

function editStaff(userId, fname, mi, lname, phone, address, email) {
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", '/Staff/EditStaff');

    function createHiddenInput(name, value) {
        var input = document.createElement("input");
        input.setAttribute("type", "hidden");
        input.setAttribute("name", name);
        input.setAttribute("value", value);
        form.appendChild(input);
    }

    createHiddenInput("userId", userId);
    createHiddenInput("fname", fname);
    createHiddenInput("mi", mi);
    createHiddenInput("lname", lname);
    createHiddenInput("phone", phone);
    createHiddenInput("address", address);
    createHiddenInput("email", email);

    document.body.appendChild(form);
    form.submit();
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