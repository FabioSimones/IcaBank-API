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

O quadrado preto significa o nome do servidor, enquanto a vemelha será o nome do seu banco de dados, que no nosso caso será: "IcaBank". Quanto ao nome do servidor você encontrará o seu acessando a Microsoft SQL Server Management Studio, que ao abrir aparecerá a seguinte tela de login:

![image](https://github.com/user-attachments/assets/fd0e625c-4186-4992-9541-c3f50460b14d)


