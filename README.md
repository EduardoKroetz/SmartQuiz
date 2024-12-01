
# QuizGenerator
Aplicação para criar, gerar e jogar Quizzes

### Relações das entidades

-> User  

---> Quiz  
-----> Question  
-------> AnswerOption  
-------> AnswerOption  

---> Matches  
-----> Quiz  
-----> Response  
-------> AnswerOption  
-----> Review  

### Partidas

#### Tempo de expiração das partidas

As partidas podem ou não ter tempo de expiração, isso depende do Quiz. O tempo é contabilizado desde a criação da partida e caso o tempo expire, não vai ser possível responder mais perguntas e a partida vai falhar (Status = "Failed").

### Quizzes

#### Criação e Ativação de Quiz

O Quiz não é ativado automaticamente ao criar e deve ser ativado através de um endpoint. O quiz só vai ser ativado caso tenha questões associadas a ele. Quizzes não ativos não seram incluídos no endpoint de pesquisa de Quiz.

## Tecnologias Utilizadas

- **Linguagem:** C#.
- **Framework:** ASP.NET Core.
- **Banco de Dados:** PostgreSQL.

## Rotas da API


## Accounts

| Método | Endpoint                                    | Descrição                                                                     |
|--------|---------------------------------------------|-------------------------------------------------------------------------------|
| POST   | `/api/Accounts/register`                    | Registrar novo usuário                                                        |
| POST   | `/api/Accounts/login`                       | Autenticar usuário com e-mail e senha                                          |
| GET    | `/api/Accounts/{userId}`                    | Buscar usuário pelo Id                                                        |
| GET    | `/api/Accounts`                             | Obter informações do usuário autenticado                                       |
| GET    | `/api/Accounts/{userId}/matches`            | Buscar partidas de um usuário                                                 |
| GET    | `/api/Accounts/{userId}/quizzes`            | Buscar quizzes de um usuário                                                  |
| POST   | `/api/Accounts/verify-email`                | Criar código de verificação de e-mail e enviar para o e-mail do usuário        |
| POST   | `/api/Accounts/verify-email-code/{code}`    | Verificar se o código de verificação de e-mail é válido e validar o e-mail     |

---

## AnswerOptions

| Método | Endpoint                                    | Descrição                                                                     |
|--------|---------------------------------------------|-------------------------------------------------------------------------------|
| POST   | `/api/AnswerOptions`                        | Criar opção de resposta para uma Questão                                      |
| DELETE | `/api/AnswerOptions/{answerOptionId}`       | Deletar opção de resposta de uma Questão                                      |

---

## Matches

| Método | Endpoint                                    | Descrição                                                                     |
|--------|---------------------------------------------|-------------------------------------------------------------------------------|
| POST   | `/api/Matches/play/quiz/{quizId}`           | Criar uma nova partida para jogar um Quiz                                     |
| POST   | `/api/Matches/{matchId}/submit/{optionId}`  | Enviar resposta para uma questão de uma partida                               |
| GET    | `/api/Matches/{matchId}/next-question`      | Buscar a próxima questão a ser respondida da partida                          |
| GET    | `/api/Matches/{matchId}`                    | Buscar detalhes da partida                                                    |
| DELETE | `/api/Matches/{matchId}`                    | Deletar partida                                                               |
| POST   | `/api/Matches/{matchId}/end`                | Finalizar partida                                                             |
| GET    | `/api/Matches/{matchId}/responses`          | Buscar todas as respostas de uma partida                                      |
| GET    | `/api/Matches`                             | Buscar partidas                                                              |

---

## Questions

| Método | Endpoint                                    | Descrição                                                                     |
|--------|---------------------------------------------|-------------------------------------------------------------------------------|
| POST   | `/api/Questions`                            | Criar uma nova questão para um Quiz                                           |
| GET    | `/api/Questions/{questionId}`               | Buscar questão pelo Id                                                        |
| DELETE | `/api/Questions/{questionId}`               | Deletar questão pelo Id                                                       |
| PATCH  | `/api/Questions/{questionId}/text`          | Atualizar o texto da pergunta                                                 |
| PATCH  | `/api/Questions/{questionId}/correct-option/{answerOptionId}` | Alterar opção de resposta correta da questão                                  |
| GET    | `/api/Questions/{questionId}/answer-options`| Buscar opções de resposta de uma Questão                                      |

---

## Quizzes

| Método | Endpoint                                    | Descrição                                                                     |
|--------|---------------------------------------------|-------------------------------------------------------------------------------|
| POST   | `/api/Quizzes`                              | Criar um novo Quiz                                                            |
| GET    | `/api/Quizzes/{quizId}`                     | Buscar dados do Quiz pelo Id                                                  |
| PUT    | `/api/Quizzes/{quizId}`                     | Atualizar as informações de um Quiz                                           |
| DELETE | `/api/Quizzes/{quizId}`                     | Deletar um Quiz pelo Id                                                       |
| GET    | `/api/Quizzes/search`                       | Pesquisar Quiz                                                                |
| GET    | `/api/Quizzes/reviews/search`               | Pesquisar Quiz por Reviews e ordenar por maior avaliação                      |
| POST   | `/api/Quizzes/toggle/{quizId}`              | Ativar/desativar o Quiz                                                       |
| GET    | `/api/Quizzes/{quizId}/questions`           | Buscar todas as questões do Quiz                                              |

---

## Reviews

| Método | Endpoint                                    | Descrição                                                                     |
|--------|---------------------------------------------|-------------------------------------------------------------------------------|
| POST   | `/api/Reviews`                              | Criar avaliação para uma partida                                              |
| DELETE | `/api/Reviews/{reviewId}`                   | Deletar avaliação de uma partida                                              |
| PUT    | `/api/Reviews/{reviewId}`                   | Atualizar avaliação                                                          |
| GET    | `/api/Reviews/{reviewId}`                   | Buscar detalhes da avaliação                                                  |
