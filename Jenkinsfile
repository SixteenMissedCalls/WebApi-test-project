pipeline {
    agent any

    environment {
        IMAGE_NAME = 'localhost:5000/currency-api'
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Docker Build') {
            steps {
                sh 'docker build -t $IMAGE_NAME .'
            }
        }

        stage('Push to Local Registry') {
            steps {
                sh 'docker push $IMAGE_NAME'
            }
        }

        stage('Run Container') {
            steps {
                sh 'docker run -d --rm -p 8081:80 --name currency-api $IMAGE_NAME'
            }
        }
    }
}
