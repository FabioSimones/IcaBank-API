# IcaBank

  Aplicação desenvolvida em .NET, com algumas funções bancárias. A escolha da linguagem .NET para o desenvolvimento desta API vem-se unicamente pela maior confiança e melhor conhecimento na linguagem e seus frameworks.
Está aplicação é unicamente com o Backend. Agora iremos ver um pouco sobre ela e suas configurações a serem realizadas para testes.
Podemos começar com os programas utilizados no desenvolvimento. Optei pela plataforma de desenvolvimento do Microsoft Visual Studio 2022, o mesmo pode ser obtido em: 
                                            https://visualstudio.microsoft.com/pt-br/vs/community/

  A aplicação foi desenvolvimento em .NET Core 3.1, o que precisará do seu kit de desenvolvimento para testes, sendo possivel baixar neste link:
                                            https://dotnet.microsoft.com/pt-br/download/dotnet/3.1

  No meu caso eu utilizei o Microsoft SQL Server Management Studio, para a criação do banco de dados. Neste caso, se você quiser adaptar a um outro sugiro que você troque frameworks para adaptar com o desejado.

#Configurações de Projetos
  Após a instalação de ambos podemos então abrir a pasta com os respectivos arquivos deste projeto, encontrando no canto direito os sguintes arquivos:

  ![image](https://github.com/user-attachments/assets/12b29a71-c66d-49c8-a241-24bd3aad5380)

  Vamos então começar configurando nosso banco de dados. Abrindo então nosso arquivo appsettings.json, encontraremos a conexão para nosso banco de dados, como podemos observar abaixo:

  ![image](https://github.com/user-attachments/assets/70a6dca7-1217-4205-97d4-4919ab0d97a9)

  O quadrado preto significa o nome do servidor, enquanto a vemelha será o nome do seu banco de dados, que no nosso caso será: "IcaBank". Quanto ao nome do servidor você encontrará o seu acessando a Microsoft SQL 
  Server Management Studio, que ao abrir aparecerá a seguinte tela de login:

  ![image](https://github.com/user-attachments/assets/fd0e625c-4186-4992-9541-c3f50460b14d)

  Após a conexão realizada podemos então partir para parte de migração de tabelas, nessa parte utilizaremos o recurso da migration onde iremos transferir os dados de tabelas criadas no Visual Studio. Nessa parte será
necessário utilizar o terminal do próprio Visual Studio, localizado na parte inferior do programa:

  ![image](https://github.com/user-attachments/assets/e86d53a7-addf-43e9-8335-b0e29ca92820)

  Observe que estou na parte do "Console do gerenciador de pacotes", aqui digitaremos primeiramente o comando: "Add-Migration MigracaoInicial", onde o nome MigracaoInicial pode ser alterado para algo de sua
preferência, e após a execução do comando utilizaremos o: "Update-Database", que servirá para criar o banco de dados com sua respectivas tabelas. Iremos notar aqui a criação de uma pasta chamada "Migrations" no 
canto da solução do projeto, está irá conter todos os comandos sql para a criação de tabelas.

![image](https://github.com/user-attachments/assets/e0a4f710-4bd6-40af-9a54-5d7e62657baf)        ![image](https://github.com/user-attachments/assets/c5a1b850-dfa5-49b9-97f5-39076bca4dbc)

#Executando e realizando testes

  Na parte superior do seu Visual Studio você poderá encontrar as seguintes opções:

  ![image](https://github.com/user-attachments/assets/f2d088dd-74ec-44fb-ae1f-725444537048)

  Sugiro que você configure da mesma maneira e então aperte a tecla do play, para que o programa execute. Está API foi desenvolvida para executar com o Swagger, que redirecionará você a pagina inicial para 
  realizarmos os testes da API. No meu caso o endereço será: "https://localhost:44359/swagger/index.html", que ao iniciar aparecerá as seguintes informações:

![image](https://github.com/user-attachments/assets/0a631246-8ac3-483b-bdf6-adaf2229e14e)        ![image](https://github.com/user-attachments/assets/d473ecaf-f452-47da-9523-62c0b8a4cf80)

#Registrando Conta

  ![image](https://github.com/user-attachments/assets/2885e07c-aab2-4cfc-9402-ba7dcac17b6d)      ![image](https://github.com/user-attachments/assets/796aad4d-ba1f-4b9e-87a0-fe5f72f52da3)

#Listando Contas
![image](https://github.com/user-attachments/assets/74038b56-1da3-4436-b7fe-f04c76bffa58)

#Autenticação de conta
  Para autenticarmos a conta será necessário o accountNumber, que é gerado aleatóriamente para cada conta criada. Ela pode ser observada na parte de listar as contas.
  Autenticação sendo realizada:
  
 ![image](https://github.com/user-attachments/assets/de4758e8-227c-428b-9736-81ecc3fe119c)       ![image](https://github.com/user-attachments/assets/6f83a3e2-5c1a-4771-b835-a6d05f5a2fd8)

 

