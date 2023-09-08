interface Environment{
    httpBackend: string;
    production: boolean;
}


export const environment:Environment = {
    production: false,
    httpBackend: 'http://localhost:5191',
};
