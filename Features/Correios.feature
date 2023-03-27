Feature: Correios

@TesteCorreios
  Scenario: Pesquisar por um CEP inexistente
    Given Eu procuro pelo CEP 80700000
    When Confirmo que o CEP não Existe
    Then Eu volto para tela incial


    Scenario: Procurar por um CEP existente
    Given Eu procuro pelo CEP 01013-001
    When Eu confirmo que o resultado é "Rua Quinze de Novembro, São Paulo/SP"
    Then Eu volto para a tela inicial

    Scenario: Procurar por um código de rastreamento inválido
    Given Eu procuro pelo código de rastreamento "SS987654321BR"
    Then Eu confirmo que o código não está correto