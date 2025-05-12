(function() {
    'use strict'

    function setupDawa(autoField) {

        let autocompleteField = autoField
        let isMultiple = autocompleteField.hasAttribute("multiple")
        let autocompleteContainer = autocompleteField.parentElement
        let modelName = autocompleteContainer.dataset.model
        let formField = autocompleteContainer.querySelector('input[name="' + modelName + '"]')
        let selectedAddressesContainer = isMultiple ? autocompleteContainer.querySelector(".selected-addresses") : null

        let selectedAddresses = []

        function removeAddress (button) {
            let item = button.parentElement
            updateAddressList(selectedAddresses.filter(x=> x.id !== item.id))

            item.style.opacity = 0
            setTimeout(() => item.remove(), 300)
        }

        function updateAddressList(addresses) {
            selectedAddresses = addresses
            formField.value = JSON.stringify(selectedAddresses)

            if (isMultiple) {
                selectedAddressesContainer.innerHTML = ''
                if(selectedAddresses.length === 0) {
                    return;
                }
                let strong = document.createElement('strong')
                strong.innerText = 'Adresser:'
                selectedAddressesContainer.appendChild(strong)

                selectedAddresses.map( x => {
                    let div = document.createElement('div')
                    div.id = x.id
                    div.dataset.model = modelName

                    let span = document.createElement('span')
                    span.innerText = x.text

                    let button = document.createElement('button')
                    button.type = 'button'
                    button.className = 'remove-btn'
                    button.onclick = removeAddress.bind(this, button)
                    button.innerHTML = '<span>X</span>'
                    div.appendChild(span)
                    div.appendChild(button)

                    selectedAddressesContainer.appendChild(div)
                });
                setTimeout(()=>autocompleteField.value = '', 30)
            }
        }

        return function(selected) {

            if(isMultiple) {
                if(!selectedAddresses.find(x => x.id === selected.data.id)) {
                    selectedAddresses.push({id: selected.data.id, text: selected.tekst})
                }
                updateAddressList(selectedAddresses)
            } else {
                updateAddressList([{id: selected.data.id, text: selected.tekst}])
            }
        }
    }

    document.querySelectorAll('.danish-addresses-autocomplete').forEach(function(field) {
        dawaAutocomplete.dawaAutocomplete(field, {
            baseUrl: "https://api.dataforsyningen.dk",
            select: setupDawa(field)
        });
    });
})();
