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


function openEditModal(userId, fname, mi, lname, department, phone, address, email, uname, pword,uimg) {  
    document.getElementById('editModal').innerHTML = `
        <div class="modal-content">
            <i class='bx bx-arrow-back' onclick="closeModal('editModal')" style="font-size: 35px !important;"></i>
            <div class="centered-text">
                <h1>Edit Staff</h1>
            </div>
            <div class="form-container">
               <div class="form-group">
                <label for="userId">ID:</label>
                <input type="text" id="userId" name="userId" class="textbox-style" style="width: 300px !important;" value="${userId}" required autofocus  oninput="updateModalContent('userId', this.value)" />
               </div>
            <div class="form-group">
                <label for="fname">Firstname:</label>
                <input type="text" id="fname" name="fname" class="textbox-style" style="width: 300px !important;" value="${fname}" required autofocus  oninput="updateModalContent('fname', this.value)" />
            </div>
            <div class="form-group">
                <label for="mi">M.I:</label>
                <input type="text" id="mi" name="mi" class="textbox-style" style="width: 300px !important;" value="${mi}" required  oninput="updateModalContent('mi', this.value)" />
            </div>
            <div class="form-group">
                <label for="lname">Lastname:</label>
                <input type="text" id="lname" name="lname" class="textbox-style" style="width: 300px !important;" value="${lname}" required  oninput="updateModalContent('lname', this.value)" />
            </div>
            <div class="form-group">
                <label for="DepartmentLabel">Department:</label>
                <input type="text" id="Department" name="Department" class="textbox-style" style="width: 300px !important;" value="${department}" required autofocus oninput="updateModalContent('department', this.value)" />
            </div>
            <div class="form-group">
                <label for="phone">Phone:</label>
                <input type="text" id="phone" name="phone" class="textbox-style" style="width: 300px !important;" value="${phone}" required oninput="updateModalContent('phone', this.value)" />
            </div>
            <div class="form-group">
                <label for="address">Address:</label>
                <input type="text" id="address" name="address" class="textbox-style" style="width: 300px !important;" value="${address}" required oninput="updateModalContent('address', this.value)" />
            </div>
            <div class="form-group">
                <label for="email">Email:</label>
                <input type="text" id="email" name="email" class="textbox-style" style="width: 300px !important;" value="${email}" required oninput="updateModalContent('email', this.value)" />
            </div>
             <div class="form-group">
                <label for="uname">User:</label>
                <input type="text" id="uname" name="uname" class="textbox-style" style="width: 300px !important;" value="${uname}" required oninput="updateModalContent('uname', this.value)" />
            </div>
            <div class="form-group">
                <label for="pword">Password:</label>
                <input type="text" id="pword" name="pword" class="textbox-style" style="width: 300px !important;" value="${pword}" requiredoninput="updateModalContent('pword', this.value)" />
            </div>
                <div class="form-group">
                    <label for="DepartmentSelect">Department:</label>
                    <select id="DepartmentSelect" name="DepartmentSelect" class="form-control">
                        <option selected disabled>${department}</option>
                        @foreach (var dep in ViewBag.DepList)
                        {
                            <option value="@dep.Value">@dep.Text</option>
                        }
                    </select>
                    <span asp-validation-for="depId" class="text-danger"></span>
                </div>
            </div>

            <div class="form-group">
                <button type="button" onclick="editStaff('${userId}')">Submit</button>
            </div>

            </br></br>
        </div>
    `;

    
    document.getElementById('editModal').style.display = 'block';
}

function editStaff(userId) {
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", '/Staff/EditStaff');

    // Create hidden inputs for each field
    function createHiddenInput(name, value) {
        var input = document.createElement("input");
        input.setAttribute("type", "hidden");
        input.setAttribute("name", name);
        input.setAttribute("value", value);
        form.appendChild(input);
    }

    // Retrieve values from the modal inputs
    var updatedUserId = document.getElementById('userId').value;
    var updatedFname = document.getElementById('fname').value;
    var updatedMi = document.getElementById('mi').value;
    var updatedLname = document.getElementById('lname').value;
    var updatedDepartment = document.getElementById('DepartmentSelect').value;
    var updatedPhone = document.getElementById('phone').value;
    var updatedAddress = document.getElementById('address').value;
    var updatedEmail = document.getElementById('email').value;
    var updatedUname = document.getElementById('uname').value;
    var updatedPword = document.getElementById('pword').value;

    // Create hidden inputs for each field
    createHiddenInput("userId", updatedUserId);
    createHiddenInput("fname", updatedFname);
    createHiddenInput("mi", updatedMi);
    createHiddenInput("lname", updatedLname);
    createHiddenInput("department", updatedDepartment);
    createHiddenInput("phone", updatedPhone);
    createHiddenInput("address", updatedAddress);
    createHiddenInput("email", updatedEmail);
    createHiddenInput("uname", updatedPword);
    createHiddenInput("pword", updatedUname);

    // Append the form to the body and submit it
    document.body.appendChild(form);
    form.submit();
}

function updateModalContent(fieldId, fieldValue) {
    // Update the value in the modal content
    document.getElementById(fieldId).value = fieldValue;
}

function openDetailModal(userId, fname, mi, lname, department, status,phone,email,address) {
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
                <p><strong>Status:</strong>${status}</p>
                <p><strong>Phone:</strong>${phone}</p
                <p><strong>Email:</strong>${email}</p>
                <p><strong>Address:</strong>${address}</p>
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