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

    document.getElementById('showSupDetailModalBtn').addEventListener('click', function () {
        console.log('showSupDetailModalBtn clicked');
        document.getElementById('supDetailModal').style.display = 'block';
    });

    document.getElementById('showEditSupModalBtn').addEventListener('click', function () {
        console.log('showEditSupModalBtn clicked');
        document.getElementById('editSupModal').style.display = 'block';
    });

    document.getElementById('closeBtn').addEventListener('click', function () {
        console.log('closeBtn clicked');
        document.getElementById('editModal').style.display = 'none';
        document.getElementById('detailModal').style.display = 'none';
        document.getElementById('supDetailModal').style.display = 'none';
        document.getElementById('editSupModal').style.display = 'none';
    });

    document.getElementById('mainCloseBtn').addEventListener('click', function () {
        console.log('mainCloseBtn clicked');
        document.getElementById('editModal').style.display = 'none';
        document.getElementById('detailModal').style.display = 'none';
        document.getElementById('supDetailModal').style.display = 'none';
        document.getElementById('editSupModal').style.display = 'none';
    });

    window.addEventListener('click', function (event) {
        console.log('window clicked');
        if (event.target == document.getElementById('editModal')) {
            document.getElementById('editModal').style.display = 'none';
        }
        if (event.target == document.getElementById('detailModal')) {
            document.getElementById('detailModal').style.display = 'none';
        }
        if (event.target == document.getElementById('supDetailModal')) {
            document.getElementById('supDetailModal').style.display = 'none';
        }
        if (event.target == document.getElementById('editSupModal')) {
            document.getElementById('editSupModal').style.display = 'none';
        }
    });
});


function openEditModal(userId, fname, mi, lname, department, phone, address, email, uname, pword) {
    document.getElementById('editModal').innerHTML = `
        <div class="modal-content">
            <i class='bx bx-arrow-back' onclick="closeModal('editModal')" style="font-size: 35px !important;"></i>
            <div class="centered-text">
                <h1>Edit Staff</h1>
            </div>
            <div class="form-container">
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

    function createHiddenInput(name, value) {
        var input = document.createElement("input");
        input.setAttribute("type", "hidden");
        input.setAttribute("name", name);
        input.setAttribute("value", value);
        form.appendChild(input);
    }

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

    document.body.appendChild(form);
    form.submit();
}


function openSupEditModal(suppid,company, fname, mi, lname, phone, address, email) {
    document.getElementById('editSupModal').innerHTML = `
        <div class="modal-content">
            <i class='bx bx-arrow-back' onclick="closeModal('editSupModal')" style="font-size: 35px !important;"></i>
            <div class="centered-text">
                <h1>Edit Supplier</h1>
            </div>
            <div class="form-container">
            
            <div class="form-group">
                <label for="company">Company:</label>
                <input type="text" id="company" name="company" class="textbox-style" style="width: 300px !important;" value="${company}" required autofocus  oninput="updateSuppModalContent('company', this.value)" />
            </div>
            <div class="form-group">
                <label for="fname">Firstname:</label>
                <input type="text" id="fname" name="fname" class="textbox-style" style="width: 300px !important;" value="${fname}" required autofocus  oninput="updateSuppModalContent('fname', this.value)" />
            </div>
            <div class="form-group">
                <label for="mi">M.I:</label>
                <input type="text" id="mi" name="mi" class="textbox-style" style="width: 300px !important;" value="${mi}" required  oninput="updateSuppModalContent('mi', this.value)" />
            </div>
            <div class="form-group">
                <label for="lname">Lastname:</label>
                <input type="text" id="lname" name="lname" class="textbox-style" style="width: 300px !important;" value="${lname}" required  oninput="updateSuppModalContent('lname', this.value)" />
            </div>
            <div class="form-group">
                <label for="phone">Phone:</label>
                <input type="text" id="phone" name="phone" class="textbox-style" style="width: 300px !important;" value="${phone}" required oninput="updateSuppModalContent('phone', this.value)" />
            </div>
            <div class="form-group">
                <label for="address">Address:</label>
                <input type="text" id="address" name="address" class="textbox-style" style="width: 300px !important;" value="${address}" required oninput="updateSuppModalContent('address', this.value)" />
            </div>
            <div class="form-group">
                <label for="email">Email:</label>
                <input type="text" id="email" name="email" class="textbox-style" style="width: 300px !important;" value="${email}" required oninput="updateSuppModalContent('email', this.value)" />
            </div>               
        </div>
            <div class="form-group">
                <button type="button" onclick="editSupplier('${suppid}')">Submit</button>
            </div>

            </br></br>
        </div>
    `;


    document.getElementById('editSupModal').style.display = 'block';
}

function editSupplier(suppid) {
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", '/Supplier/EditSupplier');

    function createHiddenInput(name, value) {
        var input = document.createElement("input");
        input.setAttribute("type", "hidden");
        input.setAttribute("name", name);
        input.setAttribute("value", value);
        form.appendChild(input);
    }

    var updatedSuppId = document.getElementById('suppid').value;
    var updatedCompany = document.getElementById('company').value;
    var updatedFname = document.getElementById('fname').value;
    var updatedMi = document.getElementById('mi').value;
    var updatedLname = document.getElementById('lname').value;
    var updatedPhone = document.getElementById('phone').value;
    var updatedAddress = document.getElementById('address').value;
    var updatedEmail = document.getElementById('email').value;

    createHiddenInput("suppid", updatedSuppId);
    createHiddenInput("company", updatedCompany);
    createHiddenInput("fname", updatedFname);
    createHiddenInput("mi", updatedMi);
    createHiddenInput("lname", updatedLname);
    createHiddenInput("phone", updatedPhone);
    createHiddenInput("address", updatedAddress);
    createHiddenInput("email", updatedEmail); 

    document.body.appendChild(form);
    form.submit();
}



function openAddProduct(prodName,prodDesc,prodPrice,supId) {
    document.getElementById('addProd').innerHTML = `
        <div class="modal-content">
            <i class='bx bx-arrow-back' onclick="closeModal('addProd')" style="font-size: 35px !important;"></i>
            <div class="centered-text">
                <h1>Add Product</h1>
            </div>
            <div class="form-container">
             <div class="form-group">
                <label for="prodN">Product Name:</label>
                <input type="text" id="prodN" name="prodN" class="textbox-style" style="width: 300px !important;" value="${prodName}" required autofocus  oninput="updateModalContent('prodName', this.value)" />
            </div>
            <div class="form-group">
                <label for="prodD">Product Description:</label>
                <input type="text" id="prodD" name="prodD" class="textbox-style" style="width: 300px !important;" value="${prodDesc}" required autofocus  oninput="updateModalContent('prodDesc', this.value)" />
            </div>
            <div class="form-group">
                <label for="prodP">Product Price:</label>
                <input type="text" id="prodP" name="prodP" class="textbox-style" style="width: 300px !important;" value="${prodPrice}" required autofocus  oninput="updateModalContent('prodPrice', this.value)" />
            </div>
            <div class="form-group">
                <label for="">M.I:</label>
                <input type="text" id="mi" name="mi" class="textbox-style" style="width: 300px !important;" value="${supId}" required  oninput="updateModalContent('supId', this.value)" />
            </div>
            <div class="form-group">
                <button type="button" onclick="addProduct()">Submit</button>
            </div>

            </br></br>
        </div>
    `;


    document.getElementById('addProd').style.display = 'block';
}


function addProduct() {
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", '/Inventory/AddProduct');

    function createHiddenInput(name, value) {
        var input = document.createElement("input");
        input.setAttribute("type", "hidden");
        input.setAttribute("name", name);
        input.setAttribute("value", value);
        form.appendChild(input);
    }

    var pName = document.getElementById('prodName').value;
    var pDesc = document.getElementById('prodDesc').value;
    var pPrice = document.getElementById('prodPrice').value;
    var sId = document.getElementById('supId').value;

    createHiddenInput("prodName", pName);
    createHiddenInput("prodDesc", pDesc);
    createHiddenInput("prodPrice", pPrice);
    createHiddenInput("supId", sId);


    document.body.appendChild(form);
    form.submit();
}

function updateModalContent(fieldId, fieldValue) {
    document.getElementById(fieldId).value = fieldValue;
}
function openDetailModal(userId, fname, mi, lname, department, status,phone,email,address) {
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
    document.getElementById('detailModal').style.display = 'block';
}

function openSupDetailModal(suppid,company, fname, mi, lname, phone, email,address) {
    document.getElementById('supDetailModal').innerHTML = `
        <div class="modal-content">
            <i class='bx bx-arrow-back' onclick="closeModal('supDetailModal')" style="font-size: 35px !important;"></i>
            
            <div class="centered-text">
                <h1>Supplier Details</h1>
            </div>
            <div class="details-container">
                <p><strong>Company:</strong> ${company}</p>
                <p><strong>Firstname:</strong> ${fname}</p>
                <p><strong>M.I:</strong> ${mi}</p>
                <p><strong>Lastname:</strong> ${lname}</p>
                <p><strong>Phone:</strong>${phone}</p
                <p><strong>Email:</strong>${email}</p>
                <p><strong>Address:</strong>${address}</p>
                <!-- Add more fields as needed -->
            </div>

            <!-- Add any additional content for viewing details -->

        </div>
    `;
    document.getElementById('supDetailModal').style.display = 'block';
}

function closeModal(modalId) {
    document.getElementById(modalId).style.display = 'none';
}