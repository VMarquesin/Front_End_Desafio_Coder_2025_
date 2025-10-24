# Operação Pato Backend

Este projeto é um sistema desenvolvido para catalogar informações sobre Patos Primordiais encontrados em nosso planeta. Utilizando tecnologia avançada, drones são enviados para coletar dados sobre esses seres únicos, e este sistema é responsável por armazenar e gerenciar essas informações.

## Estrutura do Projeto

O projeto é organizado em várias camadas, seguindo os princípios de Domain-Driven Design (DDD):

- **Domain**: Contém as entidades, agregados, objetos de valor e enums que representam o núcleo do domínio do problema.
- **Application**: Contém a lógica de aplicação, incluindo serviços e interfaces para manipulação das entidades do domínio.
- **Infrastructure**: Implementa a persistência de dados e outras operações de infraestrutura necessárias para o funcionamento do sistema.
- **API**: Camada responsável pela exposição de endpoints HTTP para interação com o sistema.
- **Tests**: Contém testes unitários para garantir a qualidade e a funcionalidade do código.

## Funcionalidades

- Catalogação de Patos Primordiais com informações como altura, peso, estado de hibernação, batimentos cardíacos, quantidade de mutações e superpoderes.
- Registro de informações dos drones, incluindo número de série, marca, fabricante, país de origem e localização.
- Conversão de unidades de medida entre diferentes sistemas (métrico e imperial).
- Armazenamento de dados de localização, incluindo cidade, país e coordenadas GPS, com precisão variável.

## Como Executar o Projeto

1. Clone o repositório.
2. Navegue até a pasta do projeto.
3. Abra a solução no seu IDE favorito.
4. Restaure as dependências e execute a aplicação.

## Contribuições

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues ou pull requests.

## Licença

Este projeto está licenciado sob a MIT License. Veja o arquivo LICENSE para mais detalhes.